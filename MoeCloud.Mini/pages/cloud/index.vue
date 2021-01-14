<template>
	<view>
		<uni-search-bar radius="100" placeholder="搜索云端文件" @confirm="search" />
		<uni-list>
			<uni-list-item :show-extra-icon="true" showArrow :extra-icon="extraIcon" :title="title" />
		</uni-list>
		<uni-fab ref="fab" :pattern="pattern" :content="content" :horizontal="horizontal" :vertical="vertical" :direction="direction" @trigger="trigger" @fabClick="fabClick" />
	</view>
</template>

<script>
	export default {
		data() {
			return {
				icon: '/static/tabs/cloud-active.png',
				title: 'Hello',
				extraIcon: {
					color: '#4cd964',
					size: '22',
					type: 'gear-filled'
				},
				directionStr: '垂直',
				horizontal: 'right',
				vertical: 'bottom',
				direction: 'horizontal',
				pattern: {
					color: '#3c3e49',
					backgroundColor: '#fff',
					selectedColor: '#007AFF',
					buttonColor:'#007AFF'
				},
				content: [{
						iconPath: '/static/component.png',
						selectedIconPath: '/static/componentHL.png',
						text: '上传',
						active: false
					},
					{
						iconPath: '/static/api.png',
						selectedIconPath: '/static/apiHL.png',
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
				console.log(e)
				this.content[e.index].active = !e.item.active
				uni.showModal({
					title: '提示',
					content: `您${this.content[e.index].active ? '选中了' : '取消了'}${e.item.text}`,
					success: function(res) {
						if (res.confirm) {
							console.log('用户点击确定')
						} else if (res.cancel) {
							console.log('用户点击取消')
						}
					}
				})
			},
			fabClick() {
				uni.showToast({
					title: '点击了悬浮按钮',
					icon: 'none'
				})
			},
			search(res) {
				uni.showToast({
					title: '搜索：' + res.value,
					icon: 'none'
				})
			},
			input(res) {
				this.searchVal = res.value
			},
			cancel(res) {
				uni.showToast({
					title: '点击取消，输入值为：' + res.value,
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
