<template>
	<view class="content">
		<uni-card :isShadow="true" title="标题内容" subTitle="副标题" mode="title" thumbnail="https://img-cdn-qiniu.dcloud.net.cn/new-page/uni.png" extra="技术没有上限" note="true" @click="clickCard">
			<view>
				<view class="image-box">
					<image class="image" mode="aspectFill" src="https://img-cdn-qiniu.dcloud.net.cn/uniapp/images/shuijiao.jpg" />
				</view>
				<view class="content-box">
					<text class="content-box-text">标题卡片带有一个双标题头部，右侧为额外描述信息 ，内容可自定义实现</text>
				</view>
			</view>
			<template slot="footer">
				<view class="footer-box">
					<view @click.stop="footerClick('喜欢')"><text class="footer-box__item">喜欢</text></view>
					<view @click.stop="footerClick('评论')"><text class="footer-box__item">评论</text></view>
					<view @click.stop="footerClick('分享')"><text class="footer-box__item">分享</text></view>
				</view>
			</template>
		</uni-card>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				info: 'This is MoeCloud!'
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
	.content {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}

	.logo {
		height: 200rpx;
		width: 200rpx;
		margin-top: 200rpx;
		margin-left: auto;
		margin-right: auto;
		margin-bottom: 50rpx;
	}

	.text-area {
		display: flex;
		justify-content: center;
	}

	.title {
		font-size: 36rpx;
		color: #8f8f94;
	}
</style>
