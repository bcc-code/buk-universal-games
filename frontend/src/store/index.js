import { createStore } from "vuex";
import { initData } from "../libs/apiHelper"

const store = createStore({
  modules: {
  },
  state: {
    loginData: {}
  },
  mutations: {
    setLoginData(state, data) {
        console.log("d changed", data)
        state.loginData = data
    }
  },
  actions: {
    async getLoginData(ctx) {
        let loginData = await initData("Start/")
        .then(r => {
          console.log(r.status)
          if(r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
            return r; 
        })

        ctx.commit("setLoginData", loginData)
        return loginData
    }
  },
});

export default store;
