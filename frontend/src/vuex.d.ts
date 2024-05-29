// import { Store } from 'vuex'

// declare module '@vue/runtime-core' {
//   // declare your own store states
//   interface State {
//     count: number
//   }

//   // provide typings for `this.$store`
//   interface ComponentCustomProperties {
//     $store: Store<State>
//   }
// }

declare module 'vuex' {
  const Vuex: any;
  export const useStore: any;
  export default Vuex;
}
