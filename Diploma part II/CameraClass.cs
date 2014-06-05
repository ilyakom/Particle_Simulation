using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace GLcamera
{
    class Camera
    {
        private struct Vector3D
        {
            public float x, y, z;
        };

        private Vector3D mPos;   //Вектор позиции камеры
        private Vector3D mView;  //Куда смотрит камера
        private Vector3D mUp;    //Вектор верхнего направления
        private Vector3D mStrafe;//Вектор для стрейфа (движения влево и вправо) камеры.

        private Vector3D Cross(Vector3D vV1, Vector3D vV2, Vector3D vVector2)
        {
            Vector3D vNormal;
            Vector3D vVector1;
            vVector1.x = vV1.x - vV2.x;
            vVector1.y = vV1.y - vV2.y;
            vVector1.z = vV1.z - vV2.z;

            // Если у нас есть 2 вектора (вектор взгляда и вертикальный вектор), 
            // у нас есть плоскость, от которой мы можем вычислить угол в 90 градусов.
            // Рассчет cross'a прост, но его сложно запомнить с первого раза. 
            // Значение X для вектора = (V1.y * V2.z) - (V1.z * V2.y)
            vNormal.x = ((vVector1.y * vVector2.z) - (vVector1.z * vVector2.y));

            // Значение Y = (V1.z * V2.x) - (V1.x * V2.z)
            vNormal.y = ((vVector1.z * vVector2.x) - (vVector1.x * vVector2.z));

            // Значение Z = (V1.x * V2.y) - (V1.y * V2.x)
            vNormal.z = ((vVector1.x * vVector2.y) - (vVector1.y * vVector2.x));

            // *ВАЖНО* Вы не можете менять этот порядок, иначе ничего не будет работать.
            // Должно быть именно так, как здесь. Просто запомните, если вы ищите Х, вы не
            // используете значение X двух векторов, и то же самое для Y и Z. Заметьте,
            // вы рассчитываете значение из двух других осей, и никогда из той же самой.

            // Итак, зачем всё это? Нам нужно найти ось, вокруг которой вращаться. Вращение камеры
            // влево и вправо простое - вертикальная ось всегда (0,1,0). 
            // Вращение камеры вверх и вниз отличается, так как оно происходит вне 
            // глобальных осей. Достаньте себе книгу по линейной алгебре, если у вас 
            // её ещё нет, она вам пригодится.

            // вернем результат.
            return vNormal;
        }

        private float Magnitude(Vector3D vNormal)
        {
            // Это даст нам величину нашей нормали, 
            // т.е. длину вектора. Мы используем эту информацию для нормализации
            // вектора. Вот формула: magnitude = sqrt(V.x^2 + V.y^2 + V.z^2)   где V - вектор.

            return (float)Math.Sqrt((vNormal.x * vNormal.x) +
                    (vNormal.y * vNormal.y) +
                    (vNormal.z * vNormal.z));
        }

        private Vector3D Normalize(Vector3D vVector)
        {
            // Вы спросите, для чего эта ф-я? Мы должны убедиться, что наш вектор нормализирован.
            // Вектор нормализирован - значит, его длинна равна 1. Например,
            // вектор (2,0,0) после нормализации будет (1,0,0).

            // Вычислим величину нормали
            float magnitude = Magnitude(vVector);

            // Теперь у нас есть величина, и мы можем разделить наш вектор на его величину.
            // Это сделает длинну вектора равной единице, так с ним будет легче работать.
            vVector.x = vVector.x / magnitude;
            vVector.y = vVector.y / magnitude;
            vVector.z = vVector.z / magnitude;

            return vVector;
        }

        public void Position_Camera(float pos_x, float pos_y, float pos_z,
                float view_x, float view_y, float view_z,
                float up_x, float up_y, float up_z)
        {
            mPos.x = pos_x;
            mPos.y = pos_y;
            mPos.z = pos_z;
            mView.x = view_x;
            mView.y = view_y;
            mView.z = view_z;
            mUp.x = up_x;
            mUp.y = up_y;
            mUp.z = up_z;
        }

        public void Rotate_View(float speed)
        {
            Vector3D vVector;// Полчим вектор взгляда
            vVector.x = mView.x - mPos.x;
            vVector.y = mView.y - mPos.y;
            vVector.z = mView.z - mPos.z;



            mView.z = (float)(mPos.z + Math.Sin(speed) * vVector.x + Math.Cos(speed) * vVector.z);
            mView.x = (float)(mPos.x + Math.Cos(speed) * vVector.x - Math.Sin(speed) * vVector.z);
        }

        public void Rotate_Position(float angle, float x, float y, float z)
        {
            mPos.x = mPos.x - mView.x;
            mPos.y = mPos.y - mView.y;
            mPos.z = mPos.z - mView.z;

            Vector3D vVector = mPos;
            Vector3D AVector;

            float SinA = (float)Math.Sin(Math.PI * angle / 180.0);
            float CosA = (float)Math.Cos(Math.PI * angle / 180.0);

            // Найдем новую позицию X для вращаемой точки 
            AVector.x = (CosA + (1 - CosA) * x * x) * vVector.x;
            AVector.x += ((1 - CosA) * x * y - z * SinA) * vVector.y;
            AVector.x += ((1 - CosA) * x * z + y * SinA) * vVector.z;

            // Найдем позицию Y 
            AVector.y = ((1 - CosA) * x * y + z * SinA) * vVector.x;
            AVector.y += (CosA + (1 - CosA) * y * y) * vVector.y;
            AVector.y += ((1 - CosA) * y * z - x * SinA) * vVector.z;

            // И позицию Z 
            AVector.z = ((1 - CosA) * x * z - y * SinA) * vVector.x;
            AVector.z += ((1 - CosA) * y * z + x * SinA) * vVector.y;
            AVector.z += (CosA + (1 - CosA) * z * z) * vVector.z;

            mPos.x = mView.x + AVector.x;
            mPos.y = mView.y + AVector.y;
            mPos.z = mView.z + AVector.z;
        }

        public void Move_Camera(float speed) //Задаем скорость
        {
            Vector3D vVector; //Получаем вектор взгляда
            vVector.x = mView.x - mPos.x;
            vVector.y = mView.y - mPos.y;
            vVector.z = mView.z - mPos.z;

            vVector.y = 0.0f; // Это запрещает камере подниматься вверх
            vVector = Normalize(vVector);

            mPos.x += vVector.x * speed;
            mPos.z += vVector.z * speed;
            mView.x += vVector.x * speed;
            mView.z += vVector.z * speed;
        }

        public void Strafe(float speed)
        {
            // добавим вектор стрейфа к позиции
            mPos.x += mStrafe.x * speed;
            mPos.z += mStrafe.z * speed;

            // Добавим теперь к взгляду
            mView.x += mStrafe.x * speed;
            mView.z += mStrafe.z * speed;
        }

        public void update()
        {
            Vector3D vCross = Cross(mView, mPos, mUp);

            //Нормализуем вектор стрейфа
            mStrafe = Normalize(vCross);
        }

        public void upDown(float speed)
        {
            mPos.y += speed;
        }

        public void Look()
        {
            Glu.gluLookAt(mPos.x, mPos.y, mPos.z, //Нами ранее обсуждаемая команда =)
                          mView.x, mView.y, mView.z,
                          mUp.x, mUp.y, mUp.z);
        }

        public double getPosX() //Возвращает позицию камеры по Х
        {
            return mPos.x;
        }

        public double getPosY() //Возвращает позицию камеры по Y
        {
            return mPos.y;
        }

        public double getPosZ() //Возвращает позицию камеры по Z
        {
            return mPos.z;
        }

        public double getViewX() //Возвращает позицию взгляда по Х
        {
            return mView.x;
        }

        public double getViewY() //Возвращает позицию взгляда по Y
        {
            return mView.y;
        }

        public double getViewZ() //Возвращает позицию взгляда по Z
        {
            return mView.z;
        }
    }
}