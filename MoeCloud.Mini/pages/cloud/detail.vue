<template>
	<view class="content">
		<image v-if="type == 'avi' || type == '3gp' || type == 'mkv' || type == 'rmvb'" src="/static/files/video.png"></image>
		<image v-else-if="type == 'doc' || type == 'docx'" src="/static/files/doc.png"></image>
		<image v-else-if="type == 'ppt' || type == 'pptx'" src="/static/files/ppt.png"></image>
		<image v-else-if="type == 'xls' || type == 'xlsx'" src="/static/files/xls.png"></image>
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
		<br /><br />
		<text>{{fileName}}</text>
		<br />
		<text style="font-size: 12px;color: #576064;">{{fileDate}}</text>
		<br />
		<text style="font-size: 14px;color: #576064;">{{fileSize}}</text>
		<br />
		<view class="btnClass">
			<button @click="viewOffice" type="primary">预览</button>
			<button type="default">重命名</button>
			<button type="warn">删除</button>
		</view>
	</view>
</template>

<script>
	export default {
		onLoad(e) {
			this.type = e.name.split('.')[1];
			this.fileName = e.name;
			// uni.setStorageSync('fileName', e.name);
			// uni.getStorageSync('fileName');
			uni.setNavigationBarTitle({
				title: e.name
			});
		},
		data() {
			return {
				type: null,
				fileSize: '3.51MB',
				fileDate: '2021-1-15 08:08'
			}
		},
		onPullDownRefresh() {
			setTimeout(function() {
				uni.stopPullDownRefresh();
			}, 1000);
		},
		methods: {
			viewOffice(current) {
				uni.setStorageSync('viewUrl', 'http%3A%2F%2Fvideo.ch9.ms%2Fbuild%2F2011%2Fslides%2FTOOL-532T_Sutter.pptx');
				uni.navigateTo({
					url: 'office?name=测试.xlsx'
				});
				// nui.previewImage({
				// 	current:,
				// 	urls:,
				// 	loop:,
				// 	indicator:,
				// })
			}			
		}
	}
</script>

<style>
	.content {
		width: 100%;
		height: 100%;
		padding-top: 180rpx;
		text-align: center;
	}

	.content image {
		width: 64px;
		height: 64px;
	}

	.btnClass {
		margin-top: 128rpx;
	}

	.btnClass button {
		margin: 32rpx;
	}
</style>
