# Example Outputs

So as the serializers within here are interchangable here is an extract from the tests which shows the same object being spat out through 5 different serializers.

## Main Take Away

- Binary will be the most efficient, but is not human readable (about 1/4 the size of JSON and 1/9 the size of XML)

- Although Json/Xml/Yaml is larger in file size, they are more compatible and human readable

- Output is generally consistent across the board other than some rounding may occur on long floating point numbers depending on the serializer

## Original C# Object

<details>
 <summary>This object is used within tests to verify behaviour works as expected.</summary>

```csharp
public static ComplexModel GeneratePopulatedModel()
{
    var a = new ComplexModel();
    a.TestValue = "WOW";
    a.NonPersisted = 100;
    a.Stuff.Add("woop");
    a.Stuff.Add("poow");

    a.NestedValue = new B
    {
        IntValue = 0,
        StringValue = "Hello",
        NestedArray = new[] { new C { FloatValue = 2.43f } }
    };

    a.NestedArray = new B[2];
    a.NestedArray[0] = new B
    {
        IntValue = 20,
        StringValue = "There",
        NestedArray = new[] { new C { FloatValue = 3.5f } }
    };

    a.NestedArray[1] = new B
    {
        IntValue = 30,
        StringValue = "Sir",
        NestedArray = new[]
        {
             new C { FloatValue = 4.1f },
             new C { FloatValue = 5.2f }
        }
    };

    a.AllTypes = new D
    {
        ByteValue = byte.MaxValue,
        ShortValue = short.MaxValue,
        IntValue = int.MaxValue,
        LongValue = long.MaxValue,
        GuidValue = Guid.NewGuid(),
        DateTimeValue = DateTime.MaxValue,
        Vector2Value = Vector2.one,
        Vector3Value = Vector3.one,
        Vector4Value = Vector4.one,
        QuaternionValue = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f),
        SomeType = SomeTypes.Known
    };

    a.SimpleDictionary.Add("key1", "some-value");
    a.SimpleDictionary.Add("key2", "some-other-value");

    a.ComplexDictionary.Add(new E { IntValue = 10 }, new C { FloatValue = 32.2f });

    return a;
}
```
</details>

---

## Binary

<details>
 <summary>About 270 bytes in size when in a file</summary>

```
15-41-73-73-65-74-73-2E-54-65-73-74-73-2E-45-64-69-74-6F-72-2E-41-03-57-4F-57-05-48-65-6C-6C-6F-00-00-00-00-01-00-00-00-1F-85-1B-40-02-00-00-00-05-54-68-65-72-65-14-00-00-00-01-00-00-00-00-00-60-40-03-53-69-72-1E-00-00-00-02-00-00-00-33-33-83-40-66-66-A6-40-02-00-00-00-04-77-6F-6F-70-04-70-6F-6F-77-FF-FF-7F-FF-FF-FF-7F-FF-FF-FF-FF-FF-FF-FF-7F-24-33-64-38-33-35-61-37-39-2D-62-61-33-33-2D-34-35-63-32-2D-38-30-32-65-2D-66-37-65-39-38-32-31-65-61-36-38-34-FF-3F-37-F4-75-28-CA-2B-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-00-00-80-3F-01-00-00-00-02-00-00-00-04-6B-65-79-31-0A-73-6F-6D-65-2D-76-61-6C-75-65-04-6B-65-79-32-10-73-6F-6D-65-2D-6F-74-68-65-72-2D-76-61-6C-75-65-01-00-00-00-0A-00-00-00-CD-CC-00-42
```
</details>

---

## Xml

<details>
 <summary>About 2.4kb when in a file</summary>

```xml
<Container>
  <TestValue>WOW</TestValue>
  <NestedValue>
    <StringValue>Hello</StringValue>
    <IntValue>0</IntValue>
    <NestedArray Count="1">
      <CollectionElement>
        <FloatValue>2.43</FloatValue>
      </CollectionElement>
    </NestedArray>
  </NestedValue>
  <NestedArray Count="2">
    <CollectionElement>
      <StringValue>There</StringValue>
      <IntValue>20</IntValue>
      <NestedArray Count="1">
        <CollectionElement>
          <FloatValue>3.5</FloatValue>
        </CollectionElement>
      </NestedArray>
    </CollectionElement>
    <CollectionElement>
      <StringValue>Sir</StringValue>
      <IntValue>30</IntValue>
      <NestedArray Count="2">
        <CollectionElement>
          <FloatValue>4.1</FloatValue>
        </CollectionElement>
        <CollectionElement>
          <FloatValue>5.2</FloatValue>
        </CollectionElement>
      </NestedArray>
    </CollectionElement>
  </NestedArray>
  <Stuff Count="2">
    <CollectionElement>woop</CollectionElement>
    <CollectionElement>poow</CollectionElement>
  </Stuff>
  <AllTypes>
    <ByteValue>255</ByteValue>
    <ShortValue>32767</ShortValue>
    <IntValue>2147483647</IntValue>
    <LongValue>9223372036854775807</LongValue>
    <GuidValue>216495ac-1e4e-412c-b6d7-071539446b76</GuidValue>
    <DateTimeValue>3155378975999999999</DateTimeValue>
    <Vector2Value>
      <x>1</x>
      <y>1</y>
    </Vector2Value>
    <Vector3Value>
      <x>1</x>
      <y>1</y>
      <z>1</z>
    </Vector3Value>
    <Vector4Value>
      <x>1</x>
      <y>1</y>
      <z>1</z>
      <w>1</w>
    </Vector4Value>
    <QuaternionValue>
      <x>1</x>
      <y>1</y>
      <z>1</z>
      <w>1</w>
    </QuaternionValue>
    <SomeType>Known</SomeType>
  </AllTypes>
  <SimpleDictionary Count="2">
    <KeyValuePair>
      <Key>key1</Key>
      <Value>some-value</Value>
    </KeyValuePair>
    <KeyValuePair>
      <Key>key2</Key>
      <Value>some-other-value</Value>
    </KeyValuePair>
  </SimpleDictionary>
  <ComplexDictionary Count="1">
    <KeyValuePair>
      <Key>
        <IntValue>10</IntValue>
      </Key>
      <Value>
        <FloatValue>32.2</FloatValue>
      </Value>
    </KeyValuePair>
  </ComplexDictionary>
  <Type>Assets.Tests.Editor.A</Type>
</Container>
```
</details>

---

## JSON

<details>
 <summary>About 1kb in size when in a file</summary>

```json
{
   "TestValue":"WOW",
   "NestedValue":{
      "StringValue":"Hello",
      "IntValue":0,
      "NestedArray":[
         {
            "FloatValue":2.4300000667572
         }
      ]
   },
   "NestedArray":[
      {
         "StringValue":"There",
         "IntValue":20,
         "NestedArray":[
            {
               "FloatValue":3.5
            }
         ]
      },
      {
         "StringValue":"Sir",
         "IntValue":30,
         "NestedArray":[
            {
               "FloatValue":4.09999990463257
            },
            {
               "FloatValue":5.19999980926514
            }
         ]
      }
   ],
   "Stuff":[
      "woop",
      "poow"
   ],
   "AllTypes":{
      "ByteValue":255,
      "ShortValue":32767,
      "IntValue":2147483647,
      "LongValue":"9223372036854775807",
      "GuidValue":"9cd0c2ea-dee4-4e0c-b703-bce71591e0c6",
      "DateTimeValue":"3155378975999999999",
      "Vector2Value":{
         "x":1,
         "y":1
      },
      "Vector3Value":{
         "x":1,
         "y":1,
         "z":1
      },
      "Vector4Value":{
         "x":1,
         "y":1,
         "z":1,
         "w":1
      },
      "QuaternionValue":{
         "x":1,
         "y":1,
         "z":1,
         "w":1
      },
      "SomeType":"Known"
   },
   "SimpleDictionary":[
      {
         "Key":"key1",
         "Value":"some-value"
      },
      {
         "Key":"key2",
         "Value":"some-other-value"
      }
   ],
   "ComplexDictionary":[
      {
         "Key":{
            "IntValue":10
         },
         "Value":{
            "FloatValue":32.2000007629395
         }
      }
   ],
   "Type":"Assets.Tests.Editor.A"
}
```
</details>

---

## BSON

<details>
 <summary>About 850 bytes in size when in a file</summary>

```
55-03-00-00-02-54-65-73-74-56-61-6C-75-65-00-04-00-00-00-57-4F-57-00-10-4E-6F-6E-50-65-72-73-69-73-74-65-64-00-64-00-00-00-03-4E-65-73-74-65-64-56-61-6C-75-65-00-58-00-00-00-02-53-74-72-69-6E-67-56-61-6C-75-65-00-06-00-00-00-48-65-6C-6C-6F-00-10-49-6E-74-56-61-6C-75-65-00-00-00-00-00-04-4E-65-73-74-65-64-41-72-72-61-79-00-21-00-00-00-03-30-00-19-00-00-00-01-46-6C-6F-61-74-56-61-6C-75-65-00-00-00-00-E0-A3-70-03-40-00-00-00-04-4E-65-73-74-65-64-41-72-72-61-79-00-D5-00-00-00-03-30-00-58-00-00-00-02-53-74-72-69-6E-67-56-61-6C-75-65-00-06-00-00-00-54-68-65-72-65-00-10-49-6E-74-56-61-6C-75-65-00-14-00-00-00-04-4E-65-73-74-65-64-41-72-72-61-79-00-21-00-00-00-03-30-00-19-00-00-00-01-46-6C-6F-61-74-56-61-6C-75-65-00-00-00-00-00-00-00-0C-40-00-00-00-03-31-00-72-00-00-00-02-53-74-72-69-6E-67-56-61-6C-75-65-00-04-00-00-00-53-69-72-00-10-49-6E-74-56-61-6C-75-65-00-1E-00-00-00-04-4E-65-73-74-65-64-41-72-72-61-79-00-3D-00-00-00-03-30-00-19-00-00-00-01-46-6C-6F-61-74-56-61-6C-75-65-00-00-00-00-60-66-66-10-40-00-03-31-00-19-00-00-00-01-46-6C-6F-61-74-56-61-6C-75-65-00-00-00-00-C0-CC-CC-14-40-00-00-00-00-04-53-74-75-66-66-00-1D-00-00-00-02-30-00-05-00-00-00-77-6F-6F-70-00-02-31-00-05-00-00-00-70-6F-6F-77-00-00-03-41-6C-6C-54-79-70-65-73-00-BC-00-00-00-12-42-79-74-65-56-61-6C-75-65-00-FF-00-00-00-00-00-00-00-12-53-68-6F-72-74-56-61-6C-75-65-00-FF-7F-00-00-00-00-00-00-10-49-6E-74-56-61-6C-75-65-00-FF-FF-FF-7F-12-4C-6F-6E-67-56-61-6C-75-65-00-FF-FF-FF-FF-FF-FF-FF-7F-05-47-75-69-64-56-61-6C-75-65-00-10-00-00-00-04-3A-F0-63-47-60-DB-12-49-B7-06-10-AF-58-AE-2C-A2-02-44-61-74-65-54-69-6D-65-56-61-6C-75-65-00-14-00-00-00-33-31-35-35-33-37-38-39-37-35-39-39-39-39-39-39-39-39-39-00-02-54-69-6D-65-53-70-61-6E-56-61-6C-75-65-00-03-00-00-00-31-30-00-12-53-6F-6D-65-54-79-70-65-00-01-00-00-00-00-00-00-00-00-04-53-69-6D-70-6C-65-44-69-63-74-69-6F-6E-61-72-79-00-63-00-00-00-03-30-00-29-00-00-00-02-4B-65-79-00-05-00-00-00-6B-65-79-31-00-02-56-61-6C-75-65-00-0B-00-00-00-73-6F-6D-65-2D-76-61-6C-75-65-00-00-03-31-00-2F-00-00-00-02-4B-65-79-00-05-00-00-00-6B-65-79-32-00-02-56-61-6C-75-65-00-11-00-00-00-73-6F-6D-65-2D-6F-74-68-65-72-2D-76-61-6C-75-65-00-00-00-04-43-6F-6D-70-6C-65-78-44-69-63-74-69-6F-6E-61-72-79-00-45-00-00-00-03-30-00-3D-00-00-00-03-4B-65-79-00-13-00-00-00-10-49-6E-74-56-61-6C-75-65-00-0A-00-00-00-00-03-56-61-6C-75-65-00-19-00-00-00-01-46-6C-6F-61-74-56-61-6C-75-65-00-00-00-00-A0-99-19-40-40-00-00-00-02-54-79-70-65-00-23-00-00-00-4C-61-7A-79-44-61-74-61-2E-54-65-73-74-73-2E-4D-6F-64-65-6C-73-2E-43-6F-6D-70-6C-65-78-4D-6F-64-65-6C-00-00
```
</details>

---

## Yaml

<details>
 <summary>About 850 bytes in size when in a file</summary>

```yaml
TestValue: WOW
NonPersisted: 100
NestedValue:
  StringValue: Hello
  IntValue: 0
  NestedArray:
  - FloatValue: 2.43000007
NestedArray:
- StringValue: There
  IntValue: 20
  NestedArray:
  - FloatValue: 3.5
- StringValue: Sir
  IntValue: 30
  NestedArray:
  - FloatValue: 4.0999999
  - FloatValue: 5.19999981
Stuff:
- woop
- poow
AllTypes:
  ByteValue: 255
  ShortValue: 32767
  IntValue: 2147483647
  LongValue: 9223372036854775807
  GuidValue: 1ebac5c9-ca69-4eba-9709-3b3fd19b213d
  DateTimeValue: 3155378975999999999
  TimeSpanValue: 10
  SomeType: Known
SimpleDictionary:
- Key: key1
  Value: some-value
- Key: key2
  Value: some-other-value
ComplexDictionary:
- Key:
    IntValue: 10
  Value:
    FloatValue: 32.2000008
Type: LazyData.Tests.Models.ComplexModel
```
</details>

---