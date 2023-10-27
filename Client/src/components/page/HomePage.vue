<template>
  <div class="container" :class="isZoom ? 'zoom-in' : ''">
    <the-header />
    <the-menu @zoomNavbar="isZoom = !isZoom" />
    <router-view />
  </div>
</template>

<script>
import TheHeader from "../TheHeader.vue";
import TheMenu from "../TheMenu.vue";
export default {
  components: {
    TheHeader,
    TheMenu,
  },
  data() {
    return {
      isZoom: false,
    };
  },
};
</script>

<style scoped></style>
