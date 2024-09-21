import { AUTH_USERINFO_URL } from '$env/static/private';
import type { LayoutServerLoad } from './$types';

export const load: LayoutServerLoad = async ({ fetch, cookies }) => {
	const accessToken = cookies.get('session_token');
	if (!accessToken) return;

	const response = await fetch(AUTH_USERINFO_URL, {
		headers: {
			Authorization: `Bearer: ${accessToken}`
		}
	});

	if (!response.ok) return;

	const { given_name: name, preferred_username: username } = await response.json();

	return {
		name,
		username
	};
};
