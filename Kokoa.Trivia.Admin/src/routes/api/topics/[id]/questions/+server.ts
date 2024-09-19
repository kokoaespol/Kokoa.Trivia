import { API_BASE_URL } from '$env/static/private';
import { error, json, type RequestHandler } from '@sveltejs/kit';

export const GET: RequestHandler = async ({ fetch, params }) => {
	const topicId = params.id;
	const response = await fetch(`${API_BASE_URL}/api/topics/${topicId}/questions`);

	if (!response.ok) {
		const err = await response.text();
		throw error(response.status, err);
	}

	const questions = await response.json();

	return json(questions);
};
