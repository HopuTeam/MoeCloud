<template>
	<view class="uni-center-content">
		<image style="margin-left:290rpx;width:81px;height:81px;background-color:#eeeeee;" mode="scaleToFill" :src="src" @error="imageError"></image>
	</view>
</template>

<script>
	export default {
		data(){
			return{
				src:'/static/files/ppt.png'
			}
		},
		onLoad(e) {
			uni.setNavigationBarTitle({
				title: e.dir
			});
		}
	}
</script>

<style>
</style>
