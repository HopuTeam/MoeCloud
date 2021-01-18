import Vue from 'vue'
import App from './App'
import {
	mapMutations
} from 'vuex'

Vue.config.productionTip = false

App.mpType = 'app'

const app = new Vue({
	...App
})
app.$mount()
