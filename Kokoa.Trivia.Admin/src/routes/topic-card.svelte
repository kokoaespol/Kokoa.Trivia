<script lang="ts">
	import { ChevronRight } from 'lucide-svelte';
	import { fly } from 'svelte/transition';
</script>

<div
	on:click
	on:keyup
	on:keydown
	role="row"
	tabindex="-1"
	transition:fly
	class="flex w-full items-center justify-between rounded-md border p-4 transition-all hover:-translate-y-1 hover:bg-primary/5 {$$props.class ||
		''}"
>
	<span class="text-xl">
		<slot></slot>
	</span>
	<span class="cursor-pointer text-white"><ChevronRight /></span>
</div>
