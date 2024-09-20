import {
	AUTH_CLIENT_ID,
	AUTH_CLIENT_SECRET,
	AUTH_CODE_URL,
	AUTH_REDIRECT_URI,
	AUTH_TOKEN_URL
} from '$env/static/private';
import { redirect, type Handle, type HandleFetch } from '@sveltejs/kit';

export const handle: Handle = async ({ event, resolve }) => {
	if (event.isSubRequest) {
		// See: https://github.com/sveltejs/kit/issues/12692
		return event.fetch(event.request);
	}

	const token = event.cookies.get('session_token');

	if (
		!token &&
		!event.url.pathname.startsWith('/api/login/callback') &&
		!event.url.pathname.startsWith('/unauthorized')
	) {
		const url = new URL(AUTH_CODE_URL);
		url.searchParams.append('client_id', AUTH_CLIENT_ID);
		url.searchParams.append('client_secret', AUTH_CLIENT_SECRET);
		url.searchParams.append('scope', 'openid');
		url.searchParams.append('response_type', 'code');
		url.searchParams.append('redirect_uri', AUTH_REDIRECT_URI);
		return redirect(302, url.toString());
	}

	return await resolve(event);
};

export const handleFetch: HandleFetch = async ({ request, fetch, event }) => {
	const accessToken = event.cookies.get('session_token');
	const refreshToken = event.cookies.get('refresh_token');

	if (accessToken) {
		request.headers.append('Authorization', 'Bearer ' + accessToken);
	}

	const body = await request.clone().arrayBuffer();
	const response = await fetch(request);

	if (response.status !== 401) {
		return response;
	}

	if (!refreshToken) {
		return redirect(302, '/');
	}

	console.debug('Access token has expired, refetching tokens with refresh token');

	const searchParams = new URLSearchParams({
		grant_type: 'refresh_token',
		client_id: AUTH_CLIENT_ID,
		client_secret: AUTH_CLIENT_SECRET,
		refresh_token: refreshToken
	});

	const refresh_response = await fetch(AUTH_TOKEN_URL, {
		method: 'POST',
		headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
		body: searchParams
	});

	if (!refresh_response.ok) {
		// Session has expired
		console.debug('Session has expired, deleting cookies and redirecting to /');
		event.cookies.delete('session_token', { path: '/' });
		event.cookies.delete('refresh_token', { path: '/' });
		return redirect(302, '/');
	}

	const { access_token, refresh_token } = await refresh_response.json();
	event.cookies.set('session_token', access_token, { path: '/' });
	event.cookies.set('refresh_token', refresh_token, { path: '/' });

	return await fetch(request.url, {
		method: request.method,
		headers: {
			...request.headers,
			Authorization: `Bearer: ${accessToken}`
		},
		body
	});
};
