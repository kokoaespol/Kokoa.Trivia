import { goto } from '$app/navigation';
import {
	AUTH_CLIENT_ID,
	AUTH_CLIENT_SECRET,
	AUTH_CODE_URL,
	AUTH_REDIRECT_URI
} from '$env/static/private';
import { redirect, type Handle } from '@sveltejs/kit';

export const handle: Handle = async ({ event, resolve }) => {
	const user = event.cookies.get('User');

	if (
		!user &&
		!event.url.pathname.startsWith('/api/login/callback') &&
		!event.url.pathname.startsWith('/unauthorized')
	) {
		console.debug('here');
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
