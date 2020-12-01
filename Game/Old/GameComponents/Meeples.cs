using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OldGameComponents;

namespace OldGameComponents {
  enum Color {
    [EnumMember(Value = "BLACK")]
    BLACK = 0,
    [EnumMember(Value = "RED")]
    RED = 1,
    [EnumMember(Value = "GREEN")] 
    GREEN = 2,
    [EnumMember(Value = "YELLOW")]
    YELLOW = 3, 
    [EnumMember(Value = "BLUE")]
    BLUE = 4
  }
  class Meeple {
    public static int maxId = 0;
    public int MeepleId { get; set; }
    public Color MeepleColor { get; set; }

    public Meeple(Color meepleColor) {
      this.MeepleId = maxId++;
      this.MeepleColor = meepleColor;
    }
  }
}