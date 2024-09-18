import { API_BASE_URL } from '$env/static/private';
import type { Topic } from '$lib/types/topic';
import { error } from '@sveltejs/kit';
import type { PageServerLoad, Actions } from './$types';
import type { Question } from '$lib/types/question';

export const load: PageServerLoad = async ({ fetch }) => {
	const topics: Topic[] = await fetch(`${API_BASE_URL}/api/topics`).then((res) => res.json());
	let questions: Question[] = [];

	if (topics.length > 0) {
		questions = await fetch(`/api/topics/${topics[0].id}/questions`).then((res) => res.json());
	}

	return {
		topics,
		questions
	};
};

export const actions: Actions = {
	createTopic: async ({ fetch, request }) => {
		const formData = await request.formData();
		const data = { name: formData.get('name') as string };

		if (data.name?.length === 0) {
			throw error(400, 'Invalid topic name');
		}

		const response = await fetch(`${API_BASE_URL}/api/topics`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(data)
		});

		if (!response.ok) {
			const err = await response.text();
			throw error(response.status, err);
		}

		return {
			message: 'Topic created successfully'
		};
	},
	createQuestion: async ({ fetch, request, url }) => {
		const topicId = url.searchParams.get('topicId');
		const formData = await request.formData();
		const data = {
			title: formData.get('title'),
			options: (formData.get('options') as string)?.split(','),
			correct_option: formData.get('correct_option')
		};
		console.log(data);
	}
};
