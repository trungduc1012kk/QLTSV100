<template>
  <div class="form-input" :class="hasIcon ? ' input-number' : ''">
    <input
      class="input text-right"
      @keyup="updateValue"
      :placeholder="label"
      :tabindex="tab"
      @blur="onBlur"
      :class="this.border"
      :disabled="disabled"
    />
    <div class="up-down" v-if="hasIcon">
      <div class="up" @click.stop="onClickUp">
        <div class="btn icon-up-bold"></div>
      </div>
      <div class="down" @click.stop="onClickDown">
        <div class="btn icon-down-bold"></div>
      </div>
    </div>
  </div>
</template>

<script>
import AutoNumeric from "autonumeric";

export default {
  props: {
    label: {
      type: String,
      default: "",
    },
    modelValue: {
      type: Number,
      default: 0,
    },
    max: {
      type: Number,
      default: 10000000000000,
    },
    decimalPlaces: {
      type: Number,
      default: 0,
    },
    tab: {
      type: String,
    },
    border: {
      type: String,
    },
    hasIcon: {
      type: Boolean,
      default: false,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
  },

  data() {
    return {
      value: "0",
    };
  },

  methods: {
    /**
     * hàm cập nhật giá trị mới
     * Author: TTDuc (29/08/2022)
     */
    updateValue() {
      try {
        this.$emit("update:modelValue", this.getRawNumberValue());
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm thực hiện gửi sự kiện ra ngoài khi blur
     * Author : TTDuc(29/08/2022)
     */
    onBlur() {
      try {
        this.$emit("onBlur");
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * lấy giá trị gốc
     * Author:TTDuc (29/08/2022)
     */
    onClickUp() {
      try {
        if (this.disabled) return;
        this.$emit("update:modelValue", this.getRawNumberValue() + 1);
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * hàm xử lý sự kiện click down
     * Author:TTDuc (29/08/2022)
     */
    onClickDown() {
      try {
        if (this.disabled) return;
        let rawNumberValue = this.getRawNumberValue();
        if (rawNumberValue >= 1) {
          this.$emit("update:modelValue", rawNumberValue - 1);
        }
      } catch (err) {
        console.log(err);
      }
    },
    /**
     * lấy giá trị gốc
     * Author:TTDuc (29/08/2022)
     */
    getRawNumberValue() {
      let rawTextValue = this._numeric.rawValue;

      let rawNumberValue = Number(rawTextValue);

      return rawNumberValue;
    },
  },

  mounted() {
    // Khởi tạo 1 biến autonumeric
    const numeric = new AutoNumeric(this.$el.querySelector(".input"), {
      digitGroupSeparator: ".",
      decimalCharacter: ",",
      maximumValue: this.max,
      minimumValue: 0,
      decimalPlaces: this.decimalPlaces,
    });

    this._numeric = numeric;

    if (this.modelValue !== 0) {
      this._numeric.set(this.modelValue);
    }
  },
  watch: {
    /**
     * set giá trị mặc định cho numeric
     * Author: TTDuc (29/08/2022)
     */
    modelValue() {
      let rawNumberValue = this.getRawNumberValue();
      if (this.modelValue != rawNumberValue) {
        this._numeric.set(this.modelValue);
      }
    },
    "_numeric.value"(newValue) {
      console.log(1);
      this.$emit("update:modelValue", newValue);
    },
  },
};
</script>
