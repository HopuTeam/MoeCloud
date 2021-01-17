<template>
	<view class="fileInfo">
		<image v-if="type == 'avi' || type == '3gp'" src="/static/files/video.png"></image>
		<image v-else-if="type == 'docx' || type == 'doc'" src="/static/files/word.png"></image>
		<image v-else-if="type == 'ppt' || type == 'pptx'" src="/static/files/ppt.png"></image>
		<image v-else-if="type == 'xls' || type == 'xlsx'" src="/static/files/xlsx.png"></image>
		<image v-else-if="type == 'gif'" src="/static/files/gif.png"></image>
		<image v-else-if="type == 'jpg'" src="/static/files/jpg.png"></image>
		<image v-else-if="type == 'mov'" src="/static/files/mov.png"></image>
		<image v-else-if="type == 'mp4'" src="/static/files/mp4.png"></image>
		<image v-else-if="type == 'pdf'" src="/static/files/pdf.png"></image>
		<image v-else-if="type == 'png'" src="/static/files/png.png"></image>
		<image v-else-if="type == 'psd'" src="/static/files/psd.png"></image>
		<image v-else-if="type == 'rar'" src="/static/files/rar.png"></image>
		<image v-else-if="type == 'svg'" src="/static/files/svg.png"></image>
		<image v-else-if="type == 'txt'" src="/static/files/txt.png"></image>
		<image v-else-if="type == 'zip'" src="/static/files/zip.png"></image>
		<image v-else-if="type == 'svg'" src="/static/files/svg.png"></image>
		<image v-else src="/static/files/unknown.png"></image>
	</view>
</template>

<script>
	export default {
		onLoad(e) {
			uni.setStorageSync('fileName', e.name);
			uni.setNavigationBarTitle({
				title: e.name
			});
		},
		data() {
			return {
				type: uni.getStorageSync('fileName').split('.')[1]
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
	.fileInfo {
		text-align: center;
	}

	.fileInfo image {
		width: 64px;
		height: 64px;
	}
</style>
