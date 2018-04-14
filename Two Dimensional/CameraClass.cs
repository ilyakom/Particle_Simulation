using System;
using Tao.OpenGl;

namespace GraphicsProject
{
	internal class Camera
    {
        private struct Vector3D
        {
            public float X, Y, Z;
        };

        private Vector3D _mPos;   //Вектор позиции камеры
        private Vector3D _mView;  //Куда смотрит камера
        private Vector3D _mUp;    //Вектор верхнего направления
        private Vector3D _mStrafe;//Вектор для стрейфа (движения влево и вправо) камеры.

        private static Vector3D Cross(Vector3D vV1, Vector3D vV2, Vector3D vVector2)
        {
            Vector3D vNormal;
            Vector3D vVector1;
            vVector1.X = vV1.X - vV2.X;
            vVector1.Y = vV1.Y - vV2.Y;
            vVector1.Z = vV1.Z - vV2.Z;

            // Если у нас есть 2 вектора (вектор взгляда и вертикальный вектор), 
            // у нас есть плоскость, от которой мы можем вычислить угол в 90 градусов.
            // Рассчет cross'a прост, но его сложно запомнить с первого раза. 
            // Значение X для вектора = (V1.y * V2.z) - (V1.z * V2.y)
            vNormal.X = ((vVector1.Y * vVector2.Z) - (vVector1.Z * vVector2.Y));

            // Значение Y = (V1.z * V2.x) - (V1.x * V2.z)
            vNormal.Y = ((vVector1.Z * vVector2.X) - (vVector1.X * vVector2.Z));

            // Значение Z = (V1.x * V2.y) - (V1.y * V2.x)
            vNormal.Z = ((vVector1.X * vVector2.Y) - (vVector1.Y * vVector2.X));

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

        private static float Magnitude(Vector3D vNormal)
        {
            // Это даст нам величину нашей нормали, 
            // т.е. длину вектора. Мы используем эту информацию для нормализации
            // вектора. Вот формула: magnitude = sqrt(V.x^2 + V.y^2 + V.z^2)   где V - вектор.

            return (float)Math.Sqrt(vNormal.X * vNormal.X +
                    vNormal.Y * vNormal.Y +
                    vNormal.Z * vNormal.Z);
        }

        private static Vector3D Normalize(Vector3D vVector)
        {
            // Вы спросите, для чего эта ф-я? Мы должны убедиться, что наш вектор нормализирован.
            // Вектор нормализирован - значит, его длинна равна 1. Например,
            // вектор (2,0,0) после нормализации будет (1,0,0).

            // Вычислим величину нормали
            var magnitude = Magnitude(vVector);

            // Теперь у нас есть величина, и мы можем разделить наш вектор на его величину.
            // Это сделает длинну вектора равной единице, так с ним будет легче работать.
            vVector.X = vVector.X / magnitude;
            vVector.Y = vVector.Y / magnitude;
            vVector.Z = vVector.Z / magnitude;

            return vVector;
        }

        public void Position_Camera(float posX, float posY, float posZ,
                float viewX, float viewY, float viewZ,
                float upX, float upY, float upZ)
        {
            _mPos.X = posX;
            _mPos.Y = posY;
            _mPos.Z = posZ;
            _mView.X = viewX;
            _mView.Y = viewY;
            _mView.Z = viewZ;
            _mUp.X = upX;
            _mUp.Y = upY;
            _mUp.Z = upZ;
        }

        public void Rotate_View(float speed)
        {
            Vector3D vVector;// Полчим вектор взгляда
            vVector.X = _mView.X - _mPos.X;
            vVector.Y = _mView.Y - _mPos.Y;
            vVector.Z = _mView.Z - _mPos.Z;

            _mView.Z = (float)(_mPos.Z + Math.Sin(speed) * vVector.X + Math.Cos(speed) * vVector.Z);
            _mView.X = (float)(_mPos.X + Math.Cos(speed) * vVector.X - Math.Sin(speed) * vVector.Z);
        }

        public void Rotate_Position(float angle, float x, float y, float z)
        {
            _mPos.X = _mPos.X - _mView.X;
            _mPos.Y = _mPos.Y - _mView.Y;
            _mPos.Z = _mPos.Z - _mView.Z;

            var vVector = _mPos;
            Vector3D aVector;

            var sinA = (float)Math.Sin(Math.PI * angle / 180.0);
            var cosA = (float)Math.Cos(Math.PI * angle / 180.0);

            // Найдем новую позицию X для вращаемой точки 
            aVector.X = (cosA + (1 - cosA) * x * x) * vVector.X;
            aVector.X += ((1 - cosA) * x * y - z * sinA) * vVector.Y;
            aVector.X += ((1 - cosA) * x * z + y * sinA) * vVector.Z;

            // Найдем позицию Y 
            aVector.Y = ((1 - cosA) * x * y + z * sinA) * vVector.X;
            aVector.Y += (cosA + (1 - cosA) * y * y) * vVector.Y;
            aVector.Y += ((1 - cosA) * y * z - x * sinA) * vVector.Z;

            // И позицию Z 
            aVector.Z = ((1 - cosA) * x * z - y * sinA) * vVector.X;
            aVector.Z += ((1 - cosA) * y * z + x * sinA) * vVector.Y;
            aVector.Z += (cosA + (1 - cosA) * z * z) * vVector.Z;

            _mPos.X = _mView.X + aVector.X;
            _mPos.Y = _mView.Y + aVector.Y;
            _mPos.Z = _mView.Z + aVector.Z;
        }

        public void Move_Camera(float speed) //Задаем скорость
        {
            Vector3D vVector; //Получаем вектор взгляда
            vVector.X = _mView.X - _mPos.X;
            vVector.Y = _mView.Y - _mPos.Y;
            vVector.Z = _mView.Z - _mPos.Z;

            vVector.Y = 0.0f; // Это запрещает камере подниматься вверх
            vVector = Normalize(vVector);

            _mPos.X += vVector.X * speed;
            _mPos.Z += vVector.Z * speed;
            _mView.X += vVector.X * speed;
            _mView.Z += vVector.Z * speed;
        }

        public void Strafe(float speed)
        {
            // добавим вектор стрейфа к позиции
            _mPos.X += _mStrafe.X * speed;
            _mPos.Z += _mStrafe.Z * speed;

            // Добавим теперь к взгляду
            _mView.X += _mStrafe.X * speed;
            _mView.Z += _mStrafe.Z * speed;
        }

        public void Update()
        {
            var vCross = Cross(_mView, _mPos, _mUp);

            //Нормализуем вектор стрейфа
            _mStrafe = Normalize(vCross);
        }

        public void UpDown(float speed)
        {
            _mPos.Y += speed;
        }

        public void Look()
        {
            Glu.gluLookAt(_mPos.X, _mPos.Y, _mPos.Z,
                          _mView.X, _mView.Y, _mView.Z,
                          _mUp.X, _mUp.Y, _mUp.Z);
        }

        public double GetPosX() //Возвращает позицию камеры по Х
        {
            return _mPos.X;
        }

        public double GetPosY() //Возвращает позицию камеры по Y
        {
            return _mPos.Y;
        }

        public double GetPosZ() //Возвращает позицию камеры по Z
        {
            return _mPos.Z;
        }

        public double GetViewX() //Возвращает позицию взгляда по Х
        {
            return _mView.X;
        }

        public double GetViewY() //Возвращает позицию взгляда по Y
        {
            return _mView.Y;
        }

        public double GetViewZ() //Возвращает позицию взгляда по Z
        {
            return _mView.Z;
        }
    }
}