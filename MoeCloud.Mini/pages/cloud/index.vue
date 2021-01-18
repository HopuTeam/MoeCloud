<template>
	<view>
		<uni-search-bar radius="100" placeholder="搜索文件" @confirm="search" />
		<uni-list>
			<uni-list-item :to="`./view?id=1&dir=文件夹`" title="文件夹" note="2021-1-15 08:08" showArrow thumb="/static/files/dir.png" />
			<uni-list-item :to="`./detail?id=1&name=测试.docx`" title="测试.docx" note="2021-1-15 08:08" thumb="/static/files/doc.png" />
		</uni-list>
		<uni-fab :pattern="pattern" :content="content" :horizontal="horizontal" :vertical="vertical" :direction="direction"
		 @trigger="trigger" />
		<!-- 提交信息 -->
		<uni-popup id="dialogInput" ref="dialogInput" type="dialog">
			<uni-popup-dialog mode="input" title="新建文件夹" placeholder="请输入文件夹名称" @confirm="dialogInputConfirm"></uni-popup-dialog>
		</uni-popup>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				horizontal: 'right',
				vertical: 'bottom',
				direction: 'horizontal',
				pattern: {
					color: '#007AFF',
					backgroundColor: '#FFF',
					selectedColor: '#007AFF',
					buttonColor: '#007AFF'
				},
				content: [{
						iconPath: '/static/fabs/upload-active.png',
						text: '上传'
						// selectedIconPath: '/static/fabs/upload-active.png',
						// active: false
					},
					{
						iconPath: '/static/fabs/new-active.png',
						text: '新建'
					}
				]
			}
		},
		onLoad() {

		},
		methods: {
			trigger(e) {
				if (e.index == 0) {
					uni.showModal({
						title: '提示',
						content: `因小程序限制，暂支持图片上传`,
						success: function(res) {
							if (res.confirm) {
								console.log('用户点击确定');
							}
						}
					})
				} else if (e.index == 1) {
					this.$refs.dialogInput.open();
				}
			},
			search(res) {
				uni.showToast({
					title: '搜索：' + res.value,
					icon: 'none'
				})
			},
			/**
			 * 输入对话框的确定事件
			 */
			dialogInputConfirm(done, val) {
				if (val == "") {
					uni.showToast({
						title: '请输入文件夹名称',
						icon: 'none'
					});
				} else {
					uni.showLoading({
						title: '请稍后'
					});
					console.log(val);
					// this.value = val;
					setTimeout(() => {
						uni.hideLoading();
						// 关闭窗口后，恢复默认内容
						done();
					}, 3000)
				}
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
