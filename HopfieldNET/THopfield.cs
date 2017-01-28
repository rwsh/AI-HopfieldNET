using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HopfieldNET
{
    class THopfield
    {
        TBoxes Boxes;

        int N;
        int M;
        double[,] w;

        public THopfield(TBoxes Boxes)
        {
            this.Boxes = Boxes;

            N = Boxes.N;
            M = Boxes.Count;

            w = new double[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    w[i, j] = 0;
                }
            }

            Learning();
        }

        void Learning()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    for (int m = 0; m < M; m++)
                    {
                        w[i, j] += Boxes[m][i] * Boxes[m][j];
                    }

                    w[i, j] /= N;
                }
            }

        }

        public TBox Find(TBox BS, int T = 1000)
        {
            TBox B = new TBox(BS);
            TBox B2 = new TBox(B);

            for (int t = 0; t < T; t++)
            {
                for (int j = 0; j < N; j++)
                {
                    double d = 0;

                    for (int i = 0; i < N; i++)
                    {
                        d += w[j, i] * B[i];
                    }

                    if (d > 0)
                    {
                        B2[j] = 1;
                    }
                    else
                    {
                        B2[j] = -1;
                    }
                }

                B = new TBox(B2);

                if (Boxes.Find(B))
                {
                    return B;
                }

            }

            return null;
        }
    }
}
