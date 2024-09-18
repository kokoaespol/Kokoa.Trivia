import { API_BASE_URL } from '$env/static/private';
import type { RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ fetch, params }) => {
	const res = await fetch(`${API_BASE_URL}/api/topics/${params.id}/questions`);
	if (!res.ok) {
		const text = await res.text();
		console.error(res.status + ':' + text);
	}
	const questions = await res.json();
	const response = JSON.stringify(questions);
	return new Response(response, { status: 200 });
};
