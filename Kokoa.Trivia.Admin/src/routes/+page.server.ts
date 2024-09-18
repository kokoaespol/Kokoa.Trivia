import { API_BASE_URL } from '$env/static/private';
import type { Topic } from '$lib/types/topic';
import type { PageServerLoad } from './$types';

export const load: PageServerLoad = async ({ fetch }) => {
	const topics: Topic[] = await fetch(`${API_BASE_URL}/api/topics`).then((res) => res.json());
	return {
		topics
	};
};
