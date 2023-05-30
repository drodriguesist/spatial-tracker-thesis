using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rail
{
    public enum Direction
    {
        foward,
        backward,
    }

    public enum MovementMode
    {
        Linear,
        Keyboard,
    }

    public class RailController : MonoBehaviour
    {
        #region properties

        #region public
        public Transform nextPosition;
        #endregion

        #region private
        [SerializeField] bool loop;
        [SerializeField] float rotationSpeed;
        [SerializeField] float speed;
        [SerializeField] float speedKeyboard;
        [SerializeField] MovementMode mode;
        [SerializeField] public Transform[] nodes;

        bool isCompleted;
        int nextPositionIdx;
        Direction previousDirection;
        Direction currentDirection;
        //Coroutine lookCoroutine;
        GameObject player;
        #endregion

        #endregion

        #region unity methods
        // Start is called before the first frame update
        void Start()
        {
            currentDirection = Direction.foward; //by default going forward
            isCompleted = false;
            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = nodes[0].position;
            speed = 7.5f;
            speedKeyboard = 70f;
            nextPositionIdx = 1;
            nextPosition = nodes[nextPositionIdx];
        }

        // Update is called once per frame
        void Update()
        {
            //not being used in the demo

            //if(loop) isCompleted = false;
            //if (mode == MovementMode.Linear)
            //{
            //    player.GetComponent<PlayerController>().SetIsWalkingKeyboard(false);
            //    MoveOnRail();
            //}
            //else
            //{
            //    player.GetComponent<PlayerController>().SetIsWalking(false);
            //    if (HandleDirectionKeyboarInput())
            //    {
            //        MoveOnRailKeyBoard();
            //    }
            //}
        }
        #endregion

        #region custom methods
        /// <summary>
        /// handle keyboard inputs to change direction
        /// </summary>
        /// <returns></returns>
        private bool HandleDirectionKeyboarInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                previousDirection = currentDirection;
                currentDirection = Direction.foward;
                player.GetComponent<PlayerController>().SetIsWalkingKeyboard(true);
                return true;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                previousDirection = currentDirection;
                currentDirection = Direction.backward;
                player.GetComponent<PlayerController>().SetIsWalkingKeyboard(true);
                return true;
            }
            player.GetComponent<PlayerController>().SetIsWalkingKeyboard(false);
            return false;
        }

        /// <summary>
        /// verify if direction has changed
        /// </summary>
        /// <returns></returns>
        private bool CheckDirectionChanged()
        {
            if (previousDirection != currentDirection) return true;
            else return false;
        }

        /// <summary>
        /// move on rail with keyboard keys
        /// </summary>
        private void MoveOnRailKeyBoard()
        {
            if (!isCompleted)
            {
                if (CheckDirectionChanged())
                {
                    if(currentDirection == Direction.backward)
                    {
                        nextPositionIdx--;
                        if (nextPositionIdx < 0) nextPositionIdx = nodes.Length - 1;
                        nextPosition = nodes[nextPositionIdx];
                    }
                    else
                    {
                        nextPositionIdx++;
                        if (nextPositionIdx > nodes.Length-1) nextPositionIdx = 0;
                        nextPosition = nodes[nextPositionIdx];
                    }
                }
                Vector3 target = (nextPosition.position - player.transform.position).normalized;
                float distance = Vector3.Distance(nextPosition.position, player.transform.position);
                if (distance < 0.9f && distance > 0.05f)
                {
                    if (currentDirection == Direction.foward) nextPositionIdx++; else nextPositionIdx--;
                    if (nextPositionIdx >= nodes.Length)
                    {
                        if (loop) nextPositionIdx = 0;
                        else
                        {
                            nextPositionIdx--;
                            player.GetComponent<PlayerController>().SetIsWalkingKeyboard(false);
                            isCompleted = true;
                            return;
                        }
                    }
                    else if (nextPositionIdx < 0)
                    {
                        if (loop) nextPositionIdx = nodes.Length - 1;
                        else
                        {
                            nextPositionIdx = 0;
                            return;
                        }
                    }
                    nextPosition = nodes[nextPositionIdx];
                }
                else
                {
                    player.GetComponent<Rigidbody>().MovePosition(player.transform.position + (target * speedKeyboard * Time.deltaTime));
                }
            }
        }

        /// <summary>
        /// move on rail automatically
        /// </summary>
        private void MoveOnRail()
        {
            if(!isCompleted)
            {
                player.GetComponent<PlayerController>().SetIsWalking(true);
                if (player.transform.position == nextPosition.position)
                {
                    nextPositionIdx++;
                    if (nextPositionIdx >= nodes.Length)
                    {
                        if (loop)
                        {
                            nextPositionIdx = 0;
                        }
                        else
                        {
                            player.GetComponent<PlayerController>().SetIsWalking(false);
                            isCompleted = true;
                            return;
                        }
                    }
                    nextPosition = nodes[nextPositionIdx];
                }
                else
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, nextPosition.position, speed * Time.deltaTime);
                }
            }
        }

        private void TurnMethod()
        {
            player.transform.Rotate(Vector3.up * 100 * Input.GetAxis("Horizontal") * Time.deltaTime);
        }
        //public void StartRotating(Transform target, Transform player)
        //{
        //    if (lookCoroutine != null)
        //    {
        //        StopCoroutine(lookCoroutine);
        //    }

        //    lookCoroutine = StartCoroutine(LookAt(target, player));
        //}

        //private IEnumerator LookAt(Transform target, Transform player)
        //{
        //    Quaternion lookRotation = Quaternion.LookRotation(target.position - player.position);
        //    float time = 0;
        //    while (time < 1)
        //    {
        //        player.rotation = Quaternion.Slerp(player.rotation, lookRotation, time);
        //        time += Time.deltaTime * rotationSpeed;
        //        yield return null;
        //    }
        //}

        //private void OnGUI()
        //{
        //    if(GUI.Button(new Rect(10,10,200,30), "Look At"))
        //    {
        //        StartRotating(nextPosition);
        //    }
        //}
        #endregion
    }
}