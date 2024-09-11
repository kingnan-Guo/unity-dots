using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
// using Unity.Jobs;



// [RequireMatchingQueriesForUpdate]
// public partial class printSystemOne : SystemBase
// {
//     // protected override void OnUpdate(){
//     //     // Entities.ForEach((ref printComponentData printData) => {
//     //     //     Debug.Log(printData);
//     //     // });
//     //         Entities
//     //             .WithName("Update_Displacement")
//     //             .ForEach(
//     //                 (ref Position position, in Velocity velocity) =>
//     //                 {
//     //                     position = new Position()
//     //                     {
//     //                         Value = position.Value + velocity.Value * dT
//     //                     };
//     //                 }
//     //             )
//     //             .ScheduleParallel();
//     // }

//         protected override void OnUpdate()
//         {
//             // float dT = SystemAPI.Time.DeltaTime;
//             // Entities.ForEach((ref printComponentData printComponentData) => {
//             //     Debug.Log("Hello World");
//             // });
//             Entities.ForEach((ref printComponentData printComponentData) => {
//                 Debug.Log(printComponentData);
//             })
               
//         }


// }



// public struct Position : IComponentData
// {
//     public float3 Value;
// }

// public struct Velocity : IComponentData
// {
//     public float3 Value;
// }

// [RequireMatchingQueriesForUpdate]
// public partial class printSystemOne : SystemBase
// {
//     protected override void OnUpdate()
//     {
//         Entities.ForEach(
//             (ref printComponentData printComponentData) =>
//             {
//                 // position = new Position()
//                 // {
//                 //     Value = position.Value + velocity.Value * dT
//                 // };
//                 // printComponentData.printData;
//                 Debug.Log(printComponentData.printData);
//             }
//         ).ScheduleParallel();
//     }
// }



