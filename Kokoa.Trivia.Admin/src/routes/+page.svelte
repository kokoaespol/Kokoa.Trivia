<script lang="ts">
	import { Button } from '$lib/components/ui/button';
	import * as Resizable from '$lib/components/ui/resizable';
	import { Plus, Trash } from 'lucide-svelte';
	import type { PageData } from './$types';
	import QuestionsPane from './questions-pane.svelte';
	import TopicCard from './topic-card.svelte';
	import { Input } from '$lib/components/ui/input';
	import type { Topic } from '$lib/types/topic';
	import { enhance } from '$app/forms';

	export let data: PageData;

	let activeTopic: Topic;
	$: activeTopic = data.topics[0];

	const onClickTopic = (ix: number) => (activeTopic = data.topics[ix]);
</script>

<h1 class="p-4 text-center text-2xl">Kokoa Trivia Admin</h1>

<Resizable.PaneGroup direction="horizontal">
	<Resizable.Pane>
		<div class="flex flex-col gap-2 p-4">
			<div
				id="toolbar"
				class="mb-2 flex items-center justify-between gap-1 rounded-sm border p-2 shadow-md"
			>
				<h2 class="mb-2">Topics</h2>
				<form class="flex items-center" action="?/createTopic" method="POST" use:enhance>
					<Input type="text" name="name" />
					<Button variant="ghost" class="text-green-400" type="submit">
						<Plus />
					</Button>
				</form>
			</div>
			{#each data.topics as topic, ix}
				<TopicCard
					class={data.topics[ix].id === activeTopic.id ? 'border-primary' : ''}
					on:click={() => onClickTopic(ix)}
				>
					{topic.name}
				</TopicCard>
			{/each}
		</div>
	</Resizable.Pane>
	<Resizable.Handle />
	<Resizable.Pane>
		<div class="p-4">
			<QuestionsPane topic={activeTopic} questions={data.questions} />
		</div>
	</Resizable.Pane>
</Resizable.PaneGroup>
