<template>
	<view class="content">
		<image src="/static/logo.png"></image>
		<form @submit="formSubmit" @reset="formReset">
			<text>账号</text>
			<view>
				<input name="input" type="text" placeholder="请输入账号" />
			</view>
			<text>密码</text>
			<view>
				<input name="input" placeholder="请输入密码" />
			</view>
		</form>
	</view>
</template>

<script>
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
</style>
