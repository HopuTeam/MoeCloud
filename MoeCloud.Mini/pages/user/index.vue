<template>
	<view>
		<view style="width: 100%;">
			<view style="text-align: center;margin: 24rpx 0px;">
				<image class="touxiang" src="/static/logo.png"></image>
				<view>{{name}}</view>
				<view class="progress-box">
					<progress percent="20" show-info activeColor="#10AEFF" stroke-width="3" />
					<!-- <text>{{Withsize}} / {{Totalsize}}</text> -->
				</view>
			</view>
		</view>
		<!-- 功能列表 -->
		<view class="Functionofthecolumn-body">
			<view class="Functionofthecolumn">
				<view style="font-size: 16px;margin-left: 20px;font-weight: bold;color: #2C405A;">功能</view>
				<view class="Functionofthecolumn-noe">
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">手机备份</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">回收站</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">我的会员</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">传输助手</text>
						</navigator>
					</view>
				</view>
				<view class="Functionofthecolumn-noe">
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">离线下载</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">闪电互传</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">清理文件</text>
						</navigator>
					</view>
					<view class="Functionofthecolumn-view">
						<navigator class="Thelistof" url="../home/index">
							<img class="Thelistof-img" src="/static/logo.png"></img>
							<text class="Thelistof-text">离线数据</text>
						</navigator>
					</view>
				</view>

			</view>
		</view>
		<!-- 设置 -->
		<view class="Setupthe-body">
			<navigator class="Setupthe-Thelistof">
				<se-text class="Setupthe-se">夜间模式</se-text>
			</navigator>
			<navigator class="Setupthe-Thelistof">
				<se-text class="Setupthe-se">设置</se-text>
			</navigator>
			<navigator class="Setupthe-Thelistof">
				<se-text class="Setupthe-se">用户反馈</se-text>
			</navigator>
			<view class="" style="margin-top: 10px;"><button style="color:#FF0000;">退出</button></view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				name: '李四',
				Withsize: '500M',
				Totalsize: '1GB'
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
	.progress-box {
		/* display: flex; */
		width: 65%;
		height: 50rpx;
		margin: 0 auto;
		/* margin-bottom: 60rpx; */
	}

	.touxiang {
		border-radius: 100px;
		border-width: 2px;
		border-style: dashed;
		border-color: #87CEEB;
		width: 64px;
		height: 64px;
	}

	/* 功能部分 */
	.Functionofthecolumn-body {
		margin: 0 auto;
		width: 100%;
		height: 250px;
	}

	.Functionofthecolumn {
		width: 90%;
		height: 220px;
		background-color: #FFFFFF;
		padding-top: 2%;
		;
		border-color: #0000FF;
		margin-left: 4%;
		box-shadow: 2px 4px 6px #A9A9A9;
		;
	}

	.Functionofthecolumn-view {
		float: left;
		margin-left: 10px;
		height: 60px;
		margin-top: 30px;
	}

	.Functionofthecolumn-noe {
		flex-direction: row;
		display: flex;
	}

	.Thelistof {
		width: 60px;
	}

	.Thelistof-img {
		width: 30px;
		height: 40px;
		margin-left: 15px;
	}

	.Thelistof-text {
		width: 100%;
		font-size: 5px;
		display: block;
		margin-left: 20px
	}

	/* 设置部分 */
	.Setupthe-body {
		width: 90%;
		height: 280px;
		background-color: #FFFFFF;
		padding-top: 2%;
		;
		border-color: #0000FF;
		margin-left: 4%;
		box-shadow: 2px 4px 6px #A9A9A9;
		margin-top: 10%;
	}

	.Setupthe-Thelistof {
		width: 95%;
		height: 60px;
		/* */
		padding-left: 5%;
		display: block;
	}

	.Setupthe-se {
		display: flex;
		width: 70px;
		height: 100%;
		font-size: 15px;
		align-items: center;
		text-align: center；
	}
</style>
