<template>
	<view>
		<uni-list-chat key="0" :title="title" avatar="/static/logo.png" :note="note">
			<view class="chat-custom-right">
				<text class="chat-custom-text">刚刚</text>
				<uni-icons type="star-filled" color="#999" size="18"></uni-icons>
			</view>
		</uni-list-chat>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				title: '老王',
				note: '刚你人跑哪去了?'
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
	.chat-custom-right {
		flex: 1;
		/* #ifndef APP-NVUE */
		display: flex;
		/* #endif */
		flex-direction: column;
		justify-content: space-between;
		align-items: flex-end;
	}
	
	.chat-custom-text {
		font-size: 12px;
		color: #999;
	}
</style>
