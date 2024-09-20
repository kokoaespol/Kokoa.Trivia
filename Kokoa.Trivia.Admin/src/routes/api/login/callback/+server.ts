import { AUTH_CLIENT_ID, AUTH_CLIENT_SECRET, AUTH_TOKEN_URL } from '$env/static/private';
import { redirect, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ url, fetch }) => {
	const code = url.searchParams.get('code');
	if (!code) {
		throw redirect(302, '/unauthorized');
	}

	console.debug(`Got code: ${code}`);

	const searchParams = new URLSearchParams({
		grant_type: 'authorization_code',
		client_id: AUTH_CLIENT_ID,
		client_secret: AUTH_CLIENT_SECRET,
		redirect_uri: url.origin,
		code
	});

	const response = await fetch(AUTH_TOKEN_URL, {
		method: 'POST',
		headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
		body: searchParams
	});

	if (!response.ok) {
		const err = await response.text();
		console.log(response.status, err);
		throw redirect(302, '/unauthorized');
	}

	const data = await response.json();
	console.log(data);

	return new Response('Deez nuts');
};
