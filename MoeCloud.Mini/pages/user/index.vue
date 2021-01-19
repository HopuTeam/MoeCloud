<template>
	<view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				title: 'Test'
			}
		},
		onLoad() {

		},
		methods: {

		},
		onPullDownRefresh() {
			setTimeout(function() {
				uni.stopPullDownRefresh();
			}, 1000);
		}
	}
</script>

<style>
	
</style>