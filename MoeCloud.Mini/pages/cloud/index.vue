<template>
	<view>
		<uni-search-bar radius="100" placeholder="搜索文件" @confirm="search" />
		<uni-list>
			<uni-list-item :to="`./view?id=1&dir=文件夹`" :title="title" note="2021-1-15 8:00" showArrow :thumb="thumb" thumb-size="base" />
			<uni-list-item :to="`./detail?id=1&name=a`" title="测试.docx" note="2021-1-15 8:00" thumb="/static/files/word.png"
			 thumb-size="base" />
		</uni-list>
		<uni-fab ref="fab" :pattern="pattern" :content="content" :horizontal="horizontal" :vertical="vertical" :direction="direction" @trigger="trigger" />
	</view>
</template>

<script>
	export default {
		data() {
			return {
				thumb: '/static/files/dir.png',
				title: '文件夹',
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
						selectedIconPath: '/static/fabs/upload-active.png',
						text: '上传',
						active: false
					},
					{
						iconPath: '/static/fabs/new-active.png',
						selectedIconPath: '/static/fabs/new-active.png',
						text: '新建',
						active: false
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
						content: `因小程序限制，仅提供图片上传`,
						success: function(res) {
							if (res.confirm) {
								console.log('用户点击确定')
							} else if (res.cancel) {
								console.log('用户点击取消')
							}
						}
					})
				}
			},
			search(res) {
				uni.showToast({
					title: '搜索：' + res.value,
					icon: 'none'
				})
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
