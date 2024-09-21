import { AUTH_CLIENT_ID, AUTH_LOGOUT_REDIRECT_URI, AUTH_LOGOUT_URL } from '$env/static/private';
import { redirect, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async (event) => {
	event.cookies.delete('session_token', { path: '/' });
	event.cookies.delete('refresh_token', { path: '/' });

	console.log('Logging out');

	// TODO: Include id_token_hint to avoid confirming logout.

	const url = new URL(AUTH_LOGOUT_URL);
	url.searchParams.append('post_logout_redirect_uri', AUTH_LOGOUT_REDIRECT_URI);
	url.searchParams.append('client_id', AUTH_CLIENT_ID);

	return redirect(302, url.toString());
};
