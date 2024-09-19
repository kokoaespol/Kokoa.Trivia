import { API_BASE_URL } from '$env/static/private';
import type { Topic } from '$lib/types/topic';
import { fail } from '@sveltejs/kit';
import type { Actions, PageServerLoad } from './$types';

export const load: PageServerLoad = async ({ fetch }) => {
	const topics: Topic[] = await fetch(`${API_BASE_URL}/api/topics`).then((res) => res.json());
	return {
		topics
	};
};

export const actions: Actions = {
	createTopic: async ({ fetch, request }) => {
		const formData = await request.formData();
		const data = { name: formData.get('name') as string };

		if (data.name?.length === 0) {
			return fail(400, { error: true, message: 'Invalid topic name' });
		}

		const response = await fetch(`${API_BASE_URL}/api/topics`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(data)
		});

		if (!response.ok) {
			const err = await response.text();
			return fail(response.status, { error: true, message: err });
		}

		return {
			success: true,
			message: 'Topic created successfully'
		};
	},
	createQuestion: async ({ fetch, request, url }) => {
		const topicId = url.searchParams.get('topicId');
		const formData = await request.formData();
		const data = {
			title: formData.get('title'),
			options: (formData.get('options') as string)?.split(','),
			correct_option: parseInt(formData.get('correct_option') as string)
		};

		if (!data.title) {
			return fail(400, {
				error: true,
				message: 'The title is mandatory'
			});
		}

		if (data.options.length < 1 || data.options.every((o) => o.length === 0)) {
			return fail(400, {
				error: true,
				message: 'The question must have at least one option'
			});
		}

		if (data.correct_option >= data.options.length || data.correct_option < 0) {
			return fail(400, {
				error: true,
				message: 'The selected option is not valid'
			});
		}

		const response = await fetch(`${API_BASE_URL}/api/topics/${topicId}/questions`, {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify(data)
		});

		if (!response.ok) {
			return fail(response.status, {
				error: true,
				message: 'There was an error creating the question'
			});
		}

		return {
			success: true,
			message: 'Question created successfully'
		};
	}
};
