<template>
	<view class="content">
		<view class="avatorWrapper">
			<view class="avator">
				<image class="img" src="/static/logo.png" mode="widthFix"></image>
			</view>
		</view>
		<view @submit="onSign" class="form">
			<view class="inputWrapper">
				<input class="input" v-model="login.account" type="text" placeholder="请输入用户名" />
			</view>
			<view class="inputWrapper">
				<input class="input" v-model="login.password" type="password" placeholder="请输入密码" />
			</view>
			<view class="loginBtn">
				<!-- <button form-type="submit">登录</button> -->
				<text>登录</text>
			</view>
			<view class="forgotBtn">
				<text>找回密码</text>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				login: {
					account: "",
					password: ""
				}
			}
		},
		onLoad() {

		},
		methods: {
			onSign: function(event) {
				uni.request({
					method: 'post',
					url: 'https://moec.echocode.club/User/Sign', //仅为示例，并非真实接口地址。
					data: {
						Account: event.detail.value.account,
						Password: event.detail.value.password
					},
					success: (res) => {
						console.log(res.data);
						this.text = 'request success';
					}
				});
			}
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
		background: url('/static/background.png');
		/* width: 100vw; */
		height: 100%;
	}

	.avatorWrapper {
		height: 30vh;
		width: 100vw;
		display: flex;
		justify-content: center;
		align-items: flex-end;
	}

	.avator {
		width: 200upx;
	}

	.avator .img {
		width: 100%;
		border-width: 2px;
		border-radius: 100px;
		border-style: dashed;
	}

	.form {
		padding: 0 100upx;
		margin-top: 80px;
		/* padding: 0 48rpx; */
	}

	.inputWrapper {
		width: 100%;
		height: 80upx;
		background: white;
		border-radius: 20px;
		box-sizing: border-box;
		padding: 0 20px;
		margin-top: 24px;
		border-width: 1px;
		/* border-color: #007AFF; */
	}

	.inputWrapper .input {
		width: 100%;
		height: 100%;
		text-align: center;
		font-size: 15px;
	}

	.loginBtn {
		width: 100%;
		height: 80upx;
		background: #77B307;
		border-radius: 50upx;
		margin-top: 50px;
		display: flex;
		justify-content: center;
		align-items: center;
	}

	.loginBtn text, .loginBtn input {
		color: white;
	}

	.forgotBtn {
		text-align: center;
		color: #007AFF;
		font-size: 15px;
		margin-top: 20px;
		padding-bottom: 200rpx;
	}
</style>
