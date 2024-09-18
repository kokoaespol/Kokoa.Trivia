<script lang="ts">
	import * as Resizable from '$lib/components/ui/resizable';
	import type { PageData } from './$types';
	import QuestionsPane from './questions-pane.svelte';
	import TopicCard from './topic-card.svelte';

	export let data: PageData;

	let selectedTopic = 0;

	const onClickTopic = (ix: number) => {
		selectedTopic = ix;
	};
</script>

<h1 class="p-4 text-center text-2xl">Kokoa Trivia Admin</h1>

<Resizable.PaneGroup direction="horizontal">
	<Resizable.Pane>
		<div class="flex flex-col gap-2 p-4">
			{#each data.topics as topic, ix}
				<TopicCard
					class={ix === selectedTopic ? 'border-primary' : ''}
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
			<QuestionsPane topic={data.topics[selectedTopic]} />
		</div>
	</Resizable.Pane>
</Resizable.PaneGroup>
