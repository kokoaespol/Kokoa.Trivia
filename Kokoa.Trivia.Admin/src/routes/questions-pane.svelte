<script lang="ts">
	import Spinner from '$lib/components/spinner.svelte';
	import Button from '$lib/components/ui/button/button.svelte';
	import type { Question } from '$lib/types/question';
	import type { Topic } from '$lib/types/topic';
	import { Plus, Trash } from 'lucide-svelte';
	import { onMount } from 'svelte';

	export let topic: Topic;

	let questions: Promise<Question[]> = Promise.resolve([]);

	onMount(() => {
		questions = fetch(`/api/topics/${topic.id}/questions`).then((res) => res.json());
	});
</script>

<div id="toolbar" class="mb-2 rounded-sm border p-1">
	<Button variant="ghost" class="text-green-400">
		<Plus />
	</Button>
	<Button variant="ghost" class="text-red-400">
		<Trash />
	</Button>
</div>

<div id="questions">
	{#await questions}
		<Spinner />
	{:then questions}
		{#each questions as question}
			{question.title}
		{/each}
	{/await}
</div>
