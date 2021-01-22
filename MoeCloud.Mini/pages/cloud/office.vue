<template>
	<view class="content">
		<web-view src="https://view.officeapps.live.com/op/view.aspx?src=http%3A%2F%2Fvideo.ch9.ms%2Fbuild%2F2011%2Fslides%2FTOOL-532T_Sutter.pptx"></web-view>
	</view>
</template>

<script>
	export default {
		onLoad(e) {
			// this.url = "https://view.officeapps.live.com/op/view.aspx?src=" + uni.getStorageSync('viewUrl');
			// uni.setStorageSync('fileName', e.name);
			// uni.getStorageSync('fileName');
			uni.setNavigationBarTitle({
				title: e.name
			});
		},
		data() {
			return {
				url: 'https://www.baidu.com/'
			}
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
