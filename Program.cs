using System.Runtime.InteropServices;
using System.Transactions;


unsafe
{
    IntegersLinkedList list = new IntegersLinkedList();

    list.Add(1);
    list.Add(3);
    list.Add(4);
    list.Add(5);

    Node* node = list.Find(1);
    Console.WriteLine("\n" + (nuint)node);

    list.Print();

    int a = list.Get(3);
    Console.WriteLine("\n" + a + "\n");

    list.Set(3, 5);
    list.Print();
}

unsafe public struct Node
{
    public int Value;
    public Node* Next;
}

unsafe public class IntegersLinkedList
{
    private Node* head;
    private Node* tail;

    public void Add(int value)
    {

        //aqui eu estou armazenando espaço na memoria do tamanho de um Node, porem ainda preciso inicializar os valores que vao nesse espaço
        Node* node = (Node*)NativeMemory.Alloc(
            (nuint)sizeof(Node)
        );

        //aqui eu to inicializando os valores do nó
        node->Value = value;
        node->Next = null;

        if (head == null)
        {
            head = node;
            tail = node;
            return;
        }

        tail->Next = node;
        tail = node;
        Node* current = head;

        //se o proximo nó não estiver vazio, o nó atual (iterador) passa a ser o proxmimo nó
        while (current->Next != null)
        {
            current = current->Next;
        }
        //quando o nó atual passa a ser o vazio(Ultimo nó criado antes da execução atual) é gravado nele o endereço do novo ultimo nó criado
        current->Next = node;
    }

    public void Remove(int value)
    {

        Node* current = head;
        while (current->Next != null)
        {
            Node* next = current->Next;
            if (current == head)
            {
                head->Value = 0;
                Console.WriteLine("Apagado");
                head = head->Next;
                NativeMemory.Free(next);
                return;
            }

            if (next->Value == value && next->Value != 0 && next->Next != null)
            {
                next->Value = 0;
                Console.WriteLine("Apagado");
                tail = current;
                current->Next = next->Next;
                NativeMemory.Free(next);
                return;
            }
            current = current->Next;
        }
    }

    public void Clear()
    {

        Node* current = head;
        Node* previousNode = head;
        while (current->Next != null)
        {

            current = current->Next;
            NativeMemory.Clear(previousNode, (nuint)sizeof(Node));
            NativeMemory.Free(previousNode);
            previousNode = current->Next;
            if (current == tail)
            {
                NativeMemory.Clear(current, (nuint)sizeof(Node));
                NativeMemory.Free(current);
            }
            return;
        }
    }

    public Node* Find(int value)
    {

        Node* current = head;
        Node* node = null;
        while (current->Next != null || current == tail)
        {

            if (current->Value == value)
            {
                node = current;
                return node;
            }
            current = current->Next;
        }
        return node;
    }

    public int Get(int value)
    {

        Node* current = head;
        while (current->Next != null || current == tail)
        {

            if (current->Value == value)
            {
                return current->Value;
            }
            current = current->Next;
        }
        return current->Value;
    }

    public int Set(int value, int newValue)
    {

        Node* current = head;
        while (current->Next != null || current == tail)
        {

            if (current->Value == value)
            {
                current->Value = newValue;
                return current->Value;
            }
            current = current->Next;
        }
        return current->Value;
    }

    public void Print()
    {
        Node* current = head;

        while (current->Next != null || current == tail)
        {
            Console.WriteLine(current->Value);
            if (current == tail)
            {
                return;
            }
            current = current->Next;
        }
    }

    private void SetTail()
    {
        Node* current = head;
        while (current->Next != null)
        {
            current = current->Next;
            if (current->Next == null)
            {
                tail = current->Next;
            }
        }
    }
}
