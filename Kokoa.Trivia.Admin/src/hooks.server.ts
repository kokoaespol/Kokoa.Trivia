import {
	APP_BASE_PATH,
	AUTH_CLIENT_ID,
	AUTH_CLIENT_SECRET,
	AUTH_CODE_URL,
	AUTH_REDIRECT_URI,
	AUTH_TOKEN_URL
} from '$env/static/private';
import { setCookie } from '$lib/utils';
import { redirect, type Handle, type HandleFetch } from '@sveltejs/kit';

export const handle: Handle = async ({ event, resolve }) => {
	if (event.isSubRequest) {
		// See: https://github.comevent.request/sveltejs/kit/issues/12692
		console.log('subRequest detected, making manual call');
		return await fetch(event.request.url, {
			method: event.request.method,
			headers: event.request.headers,
			credentials: 'include',
			body: ['GET', 'HEAD'].includes(event.request.method) ? null : await event.request.text()
		});
	}

	const token = event.cookies.get('session_token');

	if (
		!token &&
		!event.url.pathname.startsWith(`/api/login/callback`) &&
		!event.url.pathname.startsWith(`/unauthorized`) &&
		!event.url.pathname.startsWith(`/api/logout`)
	) {
		console.log(`token=(${token}); event.url=(${event.url})`);
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

	const body = await request.clone().text();
	const response = await fetch(request);

	if (response.status !== 401) {
		return response;
	}

	if (!refreshToken) {
		return redirect(302, APP_BASE_PATH);
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
		return redirect(302, APP_BASE_PATH);
	}

	const { access_token, refresh_token } = await refresh_response.json();
	setCookie(event.cookies, 'session_token', access_token);
	setCookie(event.cookies, 'refresh_token', refresh_token);

	return await fetch(request.url, {
		method: request.method,
		credentials: 'include',
		headers: (() => {
			request.headers.set('Authorization', 'Bearer ' + access_token);
			return request.headers;
		})(),
		body: ['GET', 'HEAD'].includes(request.method) ? null : body
	});
};
