import {
	APP_BASE_PATH,
	AUTH_CLIENT_ID,
	AUTH_CLIENT_SECRET,
	AUTH_REDIRECT_URI,
	AUTH_TOKEN_URL
} from '$env/static/private';
import { setCookie } from '$lib/utils';
import { redirect, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ url, fetch, cookies }) => {
	const code = url.searchParams.get('code');
	if (!code) {
		throw redirect(302, `${APP_BASE_PATH}/unauthorized`);
	}

	console.debug(`Got code: ${code}`);

	const searchParams = new URLSearchParams({
		grant_type: 'authorization_code',
		client_id: AUTH_CLIENT_ID,
		client_secret: AUTH_CLIENT_SECRET,
		redirect_uri: AUTH_REDIRECT_URI,
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
		throw redirect(302, `${APP_BASE_PATH}/unauthorized`);
	}

	const { access_token, refresh_token } = await response.json();

	setCookie(cookies, 'session_token', access_token);
	setCookie(cookies, 'refresh_token', refresh_token);

	console.log(`Login successful, redirecting to: ${APP_BASE_PATH}`);
	throw redirect(302, APP_BASE_PATH);
};
