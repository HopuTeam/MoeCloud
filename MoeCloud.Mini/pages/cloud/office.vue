<template>
	<view>
		<web-view :src="url"></web-view>
	</view>
</template>

<script>
	export default {
		onLoad(e) {
			this.url = "https://view.officeapps.live.com/op/view.aspx?src=" + uni.getStorageSync('viewUrl');
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
				location.reload();
			}, 1000);
		}
	}
</script>

<style>
</style>
