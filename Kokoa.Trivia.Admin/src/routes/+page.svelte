<script lang="ts">
	import { enhance } from '$app/forms';
	import { Button } from '$lib/components/ui/button';
	import { Input } from '$lib/components/ui/input';
	import * as Resizable from '$lib/components/ui/resizable';
	import type { Topic } from '$lib/types/topic';
	import { Plus } from 'lucide-svelte';
	import toast from 'svelte-french-toast';
	import type { ActionData, PageData } from './$types';
	import QuestionsPane from './questions-pane.svelte';
	import TopicCard from './topic-card.svelte';

	export let form: ActionData;
	export let data: PageData;

	let activeTopic: Topic;
	$: activeTopic = data.topics[0];

	const onClickTopic = (ix: number) => (activeTopic = data.topics[ix]);

	$: form?.error && toast.error(form.message);
	$: form?.success && toast.success(form.message);
</script>

<svelte:head>
	<title>Admin | KOKOA Trivia</title>
</svelte:head>

<div class="flex flex-col items-center py-8">
	<Resizable.PaneGroup direction="horizontal">
		<Resizable.Pane>
			<div class="flex flex-col gap-2 p-4">
				<div
					id="toolbar"
					class="mb-2 flex items-center justify-between gap-2 rounded-sm border p-2 shadow-md"
				>
					<h2 class="mb-2">Topics</h2>
					<form class="flex w-full items-center" action="?/createTopic" method="POST" use:enhance>
						<Input class="w-full" type="text" name="name" placeholder="New topic" />
						<Button variant="ghost" class="text-accent" type="submit">
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
				{#key activeTopic}
					<QuestionsPane topic={activeTopic} />
				{/key}
			</div>
		</Resizable.Pane>
	</Resizable.PaneGroup>
</div>
