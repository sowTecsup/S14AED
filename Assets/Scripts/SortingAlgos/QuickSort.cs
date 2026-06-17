using System;

namespace Sowtank.Collections.Sorting
{
    // QUICK SORT — "Ordenamiento rápido"
    // Nombre: en la práctica es el algoritmo más RÁPIDO para arreglos en memoria
    // (mejor constante que MergeSort, muy amigable con la caché del CPU).
    // Estrategia: elige un PIVOTE, coloca menores a su izquierda y mayores a su derecha
    // (partición), luego repite recursivamente en cada mitad.
    //
    // Cuándo usarlo:
    //   ✓ Caso general en memoria RAM — el más usado en bibliotecas estándar (Arrays.Sort en Java)
    //   ✓ Cuando no se necesita estabilidad y la memoria es ajustada
    //   ✗ Peor caso O(n²) con pivote malo en arreglos ordenados/inversos
    //      → mitigado aquí con selección de pivote por mediana de tres
    //   ✗ Datos con muchos duplicados (usar 3-way partition en ese caso)
    //
    // Mejor: O(n log n) | Promedio: O(n log n) | Peor: O(n²) | Espacio: O(log n) stack | Estable: No
    public static class QuickSort
    {
        public static void Sort<T>(T[] arr) where T : IComparable<T>
        {
            if (arr.Length <= 1) return;
            SortRecursive(arr, 0, arr.Length - 1);
        }

        private static void SortRecursive(T[] arr, int low, int high)
        {
            if (low >= high) return;

            // Insertion Sort para subarreglos pequeños — mejor rendimiento en la práctica
            if (high - low < 10)
            {
                InsertionSortSubarray(arr, low, high);
                return;
            }

            int pivotIndex = Partition(arr, low, high);
            SortRecursive(arr, low, pivotIndex - 1);
            SortRecursive(arr, pivotIndex + 1, high);
        }

        // Partición Lomuto con pivote mediana-de-tres para evitar el peor caso en arreglos ordenados
        private static int Partition(T[] arr, int low, int high)
        {
            // mediana de tres: ordena arr[low], arr[mid], arr[high] y usa el del medio como pivote
            int mid = low + (high - low) / 2;
            MedianOfThree(arr, low, mid, high);

            T pivot = arr[mid];
            (arr[mid], arr[high - 1]) = (arr[high - 1], arr[mid]); // mueve pivote al penúltimo
            int pivotPos = high - 1;

            int i = low;
            int j = pivotPos - 1;

            while (true)
            {
                while (arr[++i].CompareTo(pivot) < 0) { }  // avanza hasta encontrar elemento >= pivote
                while (j > low && arr[--j].CompareTo(pivot) > 0) { } // retrocede hasta encontrar <= pivote

                if (i >= j) break;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }

            // coloca el pivote en su posición final correcta
            (arr[i], arr[pivotPos]) = (arr[pivotPos], arr[i]);
            return i;
        }

        // ordena arr[a], arr[b], arr[c] para obtener la mediana en arr[b]
        private static void MedianOfThree(T[] arr, int a, int b, int c)
        {
            if (arr[a].CompareTo(arr[b]) > 0) (arr[a], arr[b]) = (arr[b], arr[a]);
            if (arr[a].CompareTo(arr[c]) > 0) (arr[a], arr[c]) = (arr[c], arr[a]);
            if (arr[b].CompareTo(arr[c]) > 0) (arr[b], arr[c]) = (arr[c], arr[b]);
        }

        private static void InsertionSortSubarray<T>(T[] arr, int low, int high) where T : IComparable<T>
        {
            for (int i = low + 1; i <= high; i++)
            {
                T key = arr[i];
                int j = i - 1;
                while (j >= low && arr[j].CompareTo(key) > 0)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        // Caso de uso: ordenar miles de entidades de un videojuego por distancia al jugador
        // cada frame — QuickSort es el más rápido en práctica para arreglos aleatorios grandes
        public static float[] UseCaseDemo()
        {
            float[] distances = { 12.5f, 3.1f, 8.9f, 1.2f, 15.0f, 6.7f, 4.3f, 11.1f };
            Sort(distances);
            return distances; // → 1.2 3.1 4.3 6.7 8.9 11.1 12.5 15.0
        }
    }
}
