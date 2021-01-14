<template>
	<view>
		<view class="list">
			<image class="icon">{{icon}}</image>
			<text class="title">{{title}}</text>
		</view>
		<view class="list">
			<image class="icon">{{icon}}</image>
			<text class="title">{{title}}</text>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				title: 'Hello'
			}
		},
		onLoad() {

		},
		methods: {

		},
		onPullDownRefresh() {
			setTimeout(function() {
				uni.stopPullDownRefresh();
				location.reload();
			}, 1000);
		}
	}
</script>

<style>
	.list {
		height: 24rpx;
	}
</style>
