<script lang="ts">
	import '../app.css';
	import { Toaster } from 'svelte-french-toast';
	import * as Avatar from '$lib/components/ui/avatar';
	import mascota from '$lib/assets/MASCOTA.png';
	import type { LayoutData } from './$types';
	import { LogOut } from 'lucide-svelte';
	import Button from '$lib/components/ui/button/button.svelte';
	import { PUBLIC_APP_BASE_PATH } from '$env/static/public';

	export let data: LayoutData;
</script>

<Toaster position="top-right" />

<nav
	class="fixed left-0 top-0 z-50 flex w-full items-center justify-between bg-accent px-4 py-2 shadow-lg"
>
	<div class="flex items-center">
		<Avatar.Root>
			<Avatar.Image src={mascota} alt="@kokoa" />
			<Avatar.Fallback>Kokoa</Avatar.Fallback>
		</Avatar.Root>
		<h1>Kokoa Trivia Admin</h1>
	</div>
	<div class="flex items-center">
		<span>Welcome back, {data.name}</span>
		<Button
			variant="ghost"
			on:click={() => window.location.replace(`${PUBLIC_APP_BASE_PATH}/api/logout`)}
		>
			<LogOut />
		</Button>
	</div>
</nav>

<div class="mx-auto mt-[50px] p-2 md:w-4/5">
	<slot></slot>
</div>
