<script lang="ts">
	import { enhance } from '$app/forms';
	import Button from '$lib/components/ui/button/button.svelte';
	import * as Dialog from '$lib/components/ui/dialog';
	import Input from '$lib/components/ui/input/input.svelte';
	import type { Question } from '$lib/types/question';
	import type { Topic } from '$lib/types/topic';
	import { Plus } from 'lucide-svelte';
	import { Separator } from '$lib/components/ui/separator';

	export let topic: Topic | undefined;
	export let questions: Question[];

	let option = '';
	let options = [] as string[];

	let correctOption = 0;

	const onAddOption = () => {
		if (option.length > 0) {
			options = [...options, option];
			option = '';
		}
	};
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
				use:enhance
				action="?/createQuestion&topicId={topic?.id}"
				class="flex flex-col gap-2"
			>
				<Input name="title" placeholder="Title" />
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
		{question.title}
	{:else}
		<div class="text-center w-full">Wow, such empty!</div>
	{/each}
</div>
