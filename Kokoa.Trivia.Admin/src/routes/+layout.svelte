<script lang="ts">
	import '../app.css';
	import { Toaster } from 'svelte-french-toast';
</script>

<Toaster position="top-right" />

<div class="mx-auto p-2 md:w-4/5">
	<slot></slot>
</div>
