<template>
  <div>
    <splitpanes class="default-theme" horizontal style="height: 400px">
      <pane min-size="20" max-size="70">
        <span>1</span>
      </pane>
      <pane>
        <span>2</span>
      </pane>
      <pane max-size="70">
        <span>3</span>
      </pane>
    </splitpanes>
  </div>
</template>

<script>
import { Splitpanes, Pane } from "splitpanes";
import "splitpanes/dist/splitpanes.css";

export default {
  components: {
    Splitpanes,
    Pane,
  },
};
</script>

<style></style>
