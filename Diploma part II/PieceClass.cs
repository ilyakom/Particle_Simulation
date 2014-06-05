using System;

namespace Particle
{
    class piece
    {
        private float height, weight, centre, speed;

        public piece(float w, float h, float c, float s)
        {
            if (w == 0 || h == 0)
                return;
            height = h;
            weight = w;
            centre = c;
            speed = s;
        }
        public float GetCenter()
        {
            return this.centre;
        }

        public float GetWeight()
        {
            return this.weight;
        }

        public float GetHeight()
        {
            return this.height;
        }

        public float GetSpeed()
        {
            return this.speed;
        }

        public void SetCentre(float NewCentre)
        {
            this.centre = NewCentre;
            return;
        }

        public void SetSpeed(float NewSpeed)
        {
            this.speed = NewSpeed;
            return;
        }

        public void SetWeight(float NewWeight)
        {
            this.weight = NewWeight;
            return;
        }

        public void LeftBoundAttach()
        {
            float sq;
            sq = this.height * this.weight;
            this.weight += this.centre - this.weight / 2;
            this.height = sq / this.weight;
            this.centre = this.weight / 2;
            this.speed = this.height / 2;
            return;
        }

        public void Perekritie(piece prev)
        {
            float weight, sq;
            //bool MaxUse = false;
            weight = (prev.GetCenter() + prev.GetWeight() / 2) - (this.centre - this.weight / 2);
            sq = this.height * this.weight;
            //if (this.height <= prev.GetHeight())
            //    MaxUse = true;
            this.weight -= 2 * weight;
            this.height = sq / this.weight;

            //if (this.height >= prev.GetHeight() && MaxUse == true)
            //{
            //    this.height = prev.GetHeight();
            //    this.centre -= this.weight / 2;
            //    this.weight = sq / this.height;
            //    this.centre += this.weight / 2;
            //}
            return;
        }
    }
}