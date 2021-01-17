<template>
	<view>
		<uni-search-bar radius="100" placeholder="搜索文件" @confirm="search" />
		<uni-list>
			<uni-list-item :to="`./view?id=1&dir=测试`" title="测试" note="2021-1-15 8:00" showArrow thumb="/static/files/dir.png" thumb-size="base" />
			<uni-list-item :to="`./detail?id=1&name=测试.pptx`" title="测试.pptx" note="2021-1-15 8:00" thumb="/static/files/ppt.png" thumb-size="base" />
		</uni-list>
		<uni-fab ref="fab" :pattern="pattern" :content="content" :horizontal="horizontal" :vertical="vertical" :direction="direction" @trigger="trigger" />
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
		methods: {
			search(res) {
				uni.showToast({
					title: '搜索：' + res.value,
					icon: 'none'
				})
			}
		},
		onLoad(e) {
			uni.setNavigationBarTitle({
				title: e.dir
			});
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
