<script lang="ts">
	import { enhance } from '$app/forms';
	import * as Accordion from '$lib/components/ui/accordion';
	import Button from '$lib/components/ui/button/button.svelte';
	import * as Dialog from '$lib/components/ui/dialog';
	import Input from '$lib/components/ui/input/input.svelte';
	import { Separator } from '$lib/components/ui/separator';
	import type { Question } from '$lib/types/question';
	import type { Topic } from '$lib/types/topic';
	import { Plus } from 'lucide-svelte';
	import { onMount } from 'svelte';
	import toast from 'svelte-french-toast';
	import { fade } from 'svelte/transition';

	export let topic: Topic | undefined;

	let questions: Question[] = [];

	let option = '';
	let options = [] as string[];

	let correctOption = 0;
	let loading = false;

	const onAddOption = () => {
		if (option.length > 0) {
			options = [...options, option];
			option = '';
		}
	};

	const fetchQuestions = async () => {
		if (!topic) return;

		loading = true;

		const response = await fetch(`/api/topics/${topic.id}/questions`);
		if (!response.ok) {
			const err = await response.text();
			return toast.error(err);
		}

		loading = false;
		questions = await response.json();
	};

	onMount(fetchQuestions);
</script>

<div
	id="toolbar"
	class="mb-2 flex items-center justify-between gap-1 rounded-sm border p-2 shadow-md"
>
	<h2>Questions</h2>
	<Dialog.Root>
		<Dialog.Trigger>
			<Button variant="ghost" class="text-green-400" type="submit">
				<Plus />
			</Button>
		</Dialog.Trigger>
		<Dialog.Content>
			<Dialog.Header>
				<Dialog.Title>New Question</Dialog.Title>
				<Dialog.Description>
					Create a new question. Enter new options in the input below. The correct option will be
					highlighted green.
				</Dialog.Description>
			</Dialog.Header>
			<!-- svelte-ignore a11y-no-noninteractive-element-interactions -->
			<form
				method="POST"
				action="?/createQuestion&topicId={topic?.id}"
				class="flex flex-col gap-2"
				use:enhance
			>
				<input
					name="title"
					placeholder="Title"
					class="rounded-sm border bg-background p-2 placeholder:text-muted-foreground"
				/>
				<input type="hidden" name="correct_option" value={correctOption} />
				<input type="hidden" name="options" value={options.join(',')} />
				<Separator />
				<div class="flex flex-col gap-1">
					<h3>Options</h3>
					<div class="flex w-full gap-1">
						<Input placeholder="New Option" bind:value={option} />
						<Button on:click={onAddOption} variant="outline"><Plus /></Button>
					</div>
				</div>
				<div class="flex flex-col gap-1">
					{#each options as option, ix}
						<div
							on:click={() => (correctOption = ix)}
							on:keydown
							role="row"
							tabindex="-1"
							class="rounded-sm border p-1 {correctOption == ix
								? 'border-green-400'
								: ''} transition-all"
						>
							{option}
						</div>
					{/each}
				</div>
				<Button class="w-fit self-end" type="submit">Save</Button>
			</form>
		</Dialog.Content>
	</Dialog.Root>
</div>

<div id="questions" class="w-full">
	{#each questions as question}
		<div transition:fade>
			<Accordion.Root class="px-1 py-2">
				<Accordion.Item value={question.id.toString()}>
					<Accordion.Trigger>{question.title}</Accordion.Trigger>
					<Accordion.Content>
						<div class="flex flex-col gap-1">
							{#each question.options as option, ix}
								{#if question.correct_option.id == option.id}
									<span class="text-green-500">{ix + 1}. {option.content}</span>
								{:else}
									<span>{ix + 1}. {option.content}</span>
								{/if}
							{/each}
						</div>
					</Accordion.Content>
				</Accordion.Item>
			</Accordion.Root>
		</div>
	{:else}
		{#if !loading}
			<div class="text-center w-full">Wow, such empty!</div>
		{/if}
	{/each}
</div>
