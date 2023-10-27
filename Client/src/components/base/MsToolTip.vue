<template>
  <div class="tool-tip-container">
    <slot class="slot"></slot>
    <span class="tool-tip" :class="position">{{ content }}</span>
  </div>
</template>
<script>
export default {
  props: {
    position: {
      Type: String,
      default: "right",
    },
    content: {
      Type: String,
      default: "Tool tip",
    },
  },
  data() {
    return {};
  },
};
</script>
<style scoped>
.tool-tip-container {
  position: relative;
}

.tool-tip {
  max-width: 300px;
  display: none;
  position: absolute;
  z-index: 10000;
  background-color: #000;
  color: #fff;
  border: none;
  border-radius: 4px;
  padding: 5px 11px;
  font-size: 13px;
  border: 1px solid #1d1e1f;
  white-space: nowrap;
}

.tool-tip.bottom::before {
  bottom: 100%;
  left: 50%;
  transform: translate(-50%, 50%) rotate(45deg);
}

.tool-tip.top::before {
  top: 100%;
  left: 50%;
  transform: translate(-50%, -50%) rotate(45deg);
}

.tool-tip.left::before {
  left: 100%;
  top: 50%;
  transform: translate(-50%, -50%) rotate(45deg);
}

.tool-tip::before {
  content: "";
  width: 8px;
  height: 8px;
  transform: rotate(45deg) translate(6px, 1px);
  background-color: #000;
  position: absolute;
  right: 100%;
}

.tool-tip-container:hover .tool-tip {
  display: block;
}

.top {
  bottom: calc(100% + 8px);
  left: 50%;
  transform: translateX(-50%);
}

.bottom {
  top: calc(100% + 8px);
  left: 50%;
  transform: translateX(-50%);
}

.left {
  right: calc(100% + 8px);
  top: 50%;
  transform: translateY(-50%);
}

.right {
  left: calc(100% + 8px);
  top: 50%;
  transform: translateY(-50%);
}
</style>
