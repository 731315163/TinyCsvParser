using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    public class TableTree:ITableTree
    {
        
        protected Dictionary<string, TableTree> Children;
        public ITable Table { get; }

        public TableTree(ITable table = null)
        {
            this.Table = table;
        }
        public TableTree AddChildNodeTable(string key, ITable table = null)
        {
            if (Children == null)
                Children = new Dictionary<string, TableTree>();
            var tabletree = new TableTree(table);
            Children.Add(key, tabletree);
            return tabletree;
        }
        public void  AddChildNodeTable(string key, TableTree tree)
        {
            if (Children == null)
                Children = new Dictionary<string, TableTree>();
            Children.Add(key, tree);
        }
        public TableTree GetChildNodeTable(string key)
        {
            return Children[key];
        }
        public bool ContainChildNodeTable(string key)
        {
            return Children.ContainsKey(key);
        }
        public ITable GetTable(params string[] path)
        {
            TableTree tree = this;
            foreach (var p in path)
            {
                if (tree == null)
                    break;
                tree = tree.GetChildNodeTable(p);
            }
            return tree == null ? null : tree.Table;
        }

        public void AddTable(ITable table, params string[] path)
        {
            TableTree tree = this;
            var length = path.Length - 1;
            for (int i = 0; i < length; i++)
            {
                if (tree.ContainChildNodeTable(path[i]))
                {
                    tree = tree.GetChildNodeTable(path[i]);
                }
                else
                {
                    tree = tree.AddChildNodeTable(path[i]);
                }
            }
            if (length > 0)
            {
                tree.AddChildNodeTable(path[length], table);
            }
        }

        public bool ContainTable(params string[] path)
        {
            TableTree tree = this;
            bool ret = false;
            foreach(var p in path)
            {
                if (tree.ContainChildNodeTable(p))
                {
                    tree = tree.GetChildNodeTable(p);
                    ret = true;
                }
                else
                {
                    return false;
                }
            }
            return ret;
        }
    }
}
